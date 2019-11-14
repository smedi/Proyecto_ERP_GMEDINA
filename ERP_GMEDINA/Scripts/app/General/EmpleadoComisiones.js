﻿console.log("Entro en el js EmpleadoComisiones");
//OBTENER SCRIPT DE FORMATEO DE FECHA // 
$.getScript("../Scripts/app/General/SerializeDate.js")
    .done(function (script, textStatus) {
        console.log(textStatus);
    })
    .fail(function (jqxhr, settings, exception) {
        console.log("No se pudo recuperar Script SerializeDate");
    });

//FUNCION GENERICA PARA REUTILIZAR AJAX
function _ajax(params, uri, type, callback) {
    $.ajax({
        url: uri,
        type: type,
        data: { params },
        success: function (data) {
            callback(data);
        }
    });
}

//FUNCION: CARGAR DATA Y REFRESCAR LA TABLA DEL INDEX
function cargarGridComisiones() {
    _ajax(null,
        '/EmpleadoComisiones/GetData',
        'GET',
        (data) => {
            if (data.length == 0) {
                //Validar si se genera un error al cargar de nuevo el grid
                iziToast.error({
                    title: 'Error',
                    message: 'No se pudo cargar la información, contacte al administrador',
                });
            }
            //GUARDAR EN UNA VARIABLE LA DATA OBTENIDA
            var ListaComisiones = data, template = '';
            //RECORRER DATA OBETINA Y CREAR UN "TEMPLATE" PARA REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            for (var i = 0; i < ListaComisiones.length; i++) {
                var FechaRegistro = FechaFormato(ListaComisiones[i].cc_FechaRegistro);



                //-----------------------------------VALIDACION PARA RECARGAR LA TABLA SIN AFECTAR LOS CHECKBOX
                var Check = "";
                //-----------------------------------ESTA VARIABLE GUARDA CODIGO HTML DE UN CHECKBOX, PARA ENVIARLO A LA TABLA
                if (ListaComisiones[i].cc_Pagado == true) {
                    Check = '<input type="checkbox" id="cc_Pagado" name="cc_Pagado" checked disabled>'; //-----------------------------------SE LLENA LA VARIABLE CON UN INPUT CHEQUEADO
                } else {
                    Check = '<input type="checkbox" id="cc_Pagado" name="cc_Pagado" disabled>'; //-----------------------------------SE LLENA LA VARIABLE CON UN INPUT QUE NO ESTA CHEQUEADO
                }
                //-----------------------------------
                template += '<tr data-id = "' + ListaComisiones[i].cc_Id + '">' +
                    '<td>' + ListaComisiones[i].per_Nombres + '</td>' +
                    '<td>' + ListaComisiones[i].per_Apellidos + '</td>' +
                    '<td>' + ListaComisiones[i].cin_DescripcionIngreso + '</td>' +
                    '<td>' + ListaComisiones[i].cc_Monto + '</td>' +
                    '<td>' + FechaRegistro + '</td>' +
                     '<td>' + Check + '</td>' + //-----------------------------------AQUI ENVIA LA VARIABLE
                    '<td>' +
                    '<button type="button" class="btn btn-primary btn-xs" id="btnEditarEmpleadoComisiones">Editar</button>' +
                    '<button type="button" class="btn btn-default btn-xs" id="btnDetalleEmpleadoComisiones">Detalle</button>' +
                    '</td>' +
                    '</tr>';
            }
            //REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            $('#tbodyComisiones').html(template);
        });
}


//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$(document).on("click", "#tblEmpleadoComisiones tbody tr td #btnEditarEmpleadoComisiones", function () {
    var ID = $(this).closest('tr').data('id');
    $.ajax({
        url: "/EmpleadoComisiones/Edit/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })
        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                var FechaRegistro = FechaFormato(data.cc_FechaRegistro);
               
                //-----------------------------------AQUI VALIDA EL CHECKBOX PARA PODER CARGARLO EN EL MODAL
                if (data.cc_Pagado) {
                    $('#Editar #cc_Pagado').prop('checked', true);
                } else {
                    $('#Editar #cc_Pagado').prop('checked', false);
                }//-----------------------------------

                $("#Editar #cc_Id").val(data.cc_Id);
                $("#Editar #cc_Monto").val(data.cc_Monto);
                $("#Editar #cc_FechaRegistro").val(FechaRegistro);
                $("#Editar #cc_Pagado").val(data.cc_Pagado);
                //GUARDAR EL ID DEL DROPDOWNLIST (QUE ESTA EN EL REGISTRO SELECCIONADO) QUE NECESITAREMOS PONER SELECTED EN EL DDL DEL MODAL DE EDICION
                var SelectedIdEmp = data.emp_Id;
                var SelectedIdCatIngreso = data.cin_IdIngreso;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST PARA EL MODAL
                $.ajax({
                    url: "/EmpleadoComisiones/EditGetDDLEmpleado",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Editar #emp_IdEmpleado").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Editar #emp_IdEmpleado").append("<option value=0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Editar #emp_IdEmpleado").append("<option" + (iter.Id == SelectedIdEmp ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });

                $.ajax({
                    url: "/EmpleadoComisiones/EditGetDDLIngreso",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Editar #cin_IdIngreso").empty();
                        //LLENAR EL DROPDOWNLIST
                        $.each(data, function (i, iter) {
                            $("#Editar #cin_IdIngreso").append("<option" + (iter.Id == SelectedIdCatIngreso ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });
                $("#EditarEmpleadoComisiones").modal();
            }
            else {
                //Mensaje de error si no hay data
                iziToast.error({
                    title: 'Error',
                    message: 'No se pudo cargar la información, contacte al administrador',
                });
            }
            Check = "";
        });
});
//-----------------------------------NECESITA ESTA FUNCION PARA ESCUCHAR EL CAMBIO EN EL CHECKBOX Y CAMBIAR SU VALOR
$('#Editar #cc_Pagado').click(function () {
    if ($('#Editar #cc_Pagado').is(':checked')) {
        $('#Editar #cc_Pagado').val(true);
    }
    else {
        $('#Editar #cc_Pagado').val(false);
    }
});//-----------------------------------

//EJECUTAR EDICIÓN DEL REGISTRO EN EL MODAL
$("#btnUpdateComisiones").click(function () {
    //SERIALIZAR EL FORMULARIO (QUE ESTÁ EN LA VISTA PARCIAL) DEL MODAL, SE PARSEA A FORMATO JSON
    var data = $("#frmEmpleadoComisiones").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/EmpleadoComisiones/Edit",
        method: "POST",
        data: data
    }).done(function (data) {
        if (data == "error") {
            //Cuando traiga un error del backend al guardar la edicion
            iziToast.error({
                title: 'Error',
                message: 'No se pudo editar el registro, contacte al administrador',
            });
        }
        else {
            // REFRESCAR UNICAMENTE LA TABLA
            cargarGridComisiones();
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#EditarEmpleadoComisiones").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue editado de forma exitosa!',
            });
        }
    });
});

//FUNCION: PRIMERA FASE DE AGREGAR UN NUEVO REGISTRO, MOSTRAR MODAL DE CREATE
$(document).on("click", "#btnAgregarEmpleadoComisiones", function () {
    //PEDIR DATA PARA LLENAR EL DROPDOWNLIST DEL MODAL
    $.ajax({
        url: "/EmpleadoComisiones/EditGetDDLEmpleado",
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    })
        //LLENAR EL DROPDONWLIST DEL MODAL CON LA DATA OBTENIDA
        .done(function (data) {
            console.log(data);
            $("#Crear #emp_IdEmpleado").empty();
            $("#Crear #emp_IdEmpleado").append("<option value='0'>Selecione una opción...</option>");
            $.each(data, function (i, iter) {
                $("#Crear #emp_IdEmpleado").append("<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
            });
        });
    //MOSTRAR EL MODAL DE AGREGAR
    $("#AgregarEmpleadoComisiones").modal();
});

//FUNCION: PRIMERA FASE DE AGREGAR UN NUEVO REGISTRO, MOSTRAR MODAL DE CREATE
$(document).on("click", "#btnAgregarEmpleadoComisiones", function () {
    //PEDIR DATA PARA LLENAR EL DROPDOWNLIST DEL MODAL
    $.ajax({
        url: "/EmpleadoComisiones/EditGetDDLIngreso",
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    })
        //LLENAR EL DROPDONWLIST DEL MODAL CON LA DATA OBTENIDA
        .done(function (data) {
            console.log(data);
            //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
            $("#Crear #cin_IdIngreso").empty();
            //LLENAR EL DROPDOWNLIST
            $.each(data, function (i, iter) {
                $("#Crear #cin_IdIngreso").append("<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
            });
        });
    //MOSTRAR EL MODAL DE AGREGAR
    $("#AgregarEmpleadoComisiones").modal();
});
//FUNCION: CREAR EL NUEVO REGISTRO
$('#btnCreateRegistroComisiones').click(function () {
    // SIEMPRE HACER LAS RESPECTIVAS VALIDACIONES DEL LADO DEL CLIENTE

    //SERIALIZAR EL FORMULARIO DEL MODAL (ESTÁ EN LA VISTA PARCIAL)
    var data = $("#frmEmpleadoComisionesCreate").serializeArray();
    console.log(data);
    //ENVIAR DATA AL SERVIDOR PARA EJECUTAR LA INSERCIÓN
    $.ajax({
        url: "/EmpleadoComisiones/Create",
        method: "POST",
        data: data
    }).done(function (data) {
        //CERRAR EL MODAL DE AGREGAR
        $("#AgregarEmpleadoComisiones").modal('hide');
        //VALIDAR RESPUESTA OBETNIDA DEL SERVIDOR, SI LA INSERCIÓN FUE EXITOSA O HUBO ALGÚN ERROR
        if (data == "error") {
            iziToast.error({
                title: 'Error',
                message: 'No se pudo guardar el registro, contacte al administrador',
            });
        }
        else {
            cargarGridComisiones();
            // Mensaje de exito cuando un registro se ha guardado bien
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue registrado de forma exitosa!',
            });
        }
    });
});


//FUNCION: Detail 
///////////////////////////////////////////////////////////////////////////////////////////////////
$(document).on("click", "#tblEmpleadoComisiones tbody tr td #btnDetalleEmpleadoComisiones", function () {
    
    var ID = $(this).closest('tr').data('id');
    $.ajax({
        url: "/EmpleadoComisiones/Details/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })
        .done(function (data){
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                var FechaRegistro = FechaFormato(data.cc_FechaRegistro);
                var FechaCrea = FechaFormato(data.cc_FechaCrea);
                var FechaModifica = FechaFormato(data.cc_FechaModifica);
              
               // -----------------------------------AQUI VALIDA EL CHECKBOX PARA PODER CARGARLO EN EL MODAL
                if (data.cc_Pagado) {
                    $('#Detallar #cc_Pagado').prop('checked', true);
                } else {
                    $('#Detallar #cc_Pagado').prop('checked', false);
                }//-----------------------------------
                $("#Detallar #cc_Id").val(data.cc_Id);
                $("#Detallar #cc_Monto").val(data.cc_Monto);
                $("#Detallar #cc_FechaRegistro").val(FechaRegistro);
                $("#Detallar #cc_Pagado").val(cc_Pagado);
                $("#Detallar #cc_UsuarioCrea").val(data.cc_UsuarioCrea);
                $("#Detallar #cc_FechaCrea").val(FechaCrea);
                $("#Detallar #cc_UsuarioModifica").val(data.cc_UsuarioModifica);
                $("#Detallar #cc_FechaModifica").val(FechaModifica);
                //GUARDAR EL ID DEL DROPDOWNLIST (QUE ESTA EN EL REGISTRO SELECCIONADO) QUE NECESITAREMOS PONER SELECTED EN EL DDL DEL MODAL DE EDICION
                var SelectedIdEmp = data.emp_Id;
                var SelectedIdCatIngreso = data.cin_IdIngreso;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST PARA EL MODAL
                $.ajax({
                    url: "/EmpleadoComisiones/EditGetDDLEmpleado",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Detallar #emp_IdEmpleado").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Detallar #emp_IdEmpleado").append("<option value=0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Detallar #emp_IdEmpleado").append("<option" + (iter.Id == SelectedIdEmp ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });

                $.ajax({
                    url: "/EmpleadoComisiones/EditGetDDLIngreso",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Detallar #cin_IdIngreso").empty();
                        //LLENAR EL DROPDOWNLIST

                        $.each(data, function (i, iter) {
                            $("#Detallar #cin_IdIngreso").append("<option" + (iter.Id == SelectedIdCatIngreso ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });
                console.log('holaentro?')
                $("#DetalleEmpleadoComisiones").modal();
            }
            else {
                //Mensaje de error si no hay data
                iziToast.error({
                    title: 'Error',
                    message: 'No se pudo cargar la información, contacte al administrador',
                });
            }
        });
});

///////////////////////////////////////////////////////////////////////////////////////////////////



$(document).on("click", "#btnInactivarEmpleadoComisiones", function () {
    //MOSTRAR EL MODAL DE INACTIVAR
    $("#InactivarEmpleadoComisiones").modal();
});

//EJECUTAR INACTIVACION DEL REGISTRO EN EL MODAL
$("#btnInactivarRegistroComisiones").click(function () {

    var data = $("#frmEmpleadoComisionesInactivar").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/EmpleadoComisiones/Inactivar",
        method: "POST",
        data: data
    }).done(function (data) {
        if (data == "error") {
            //Cuando traiga un error del backend al guardar la edicion
            iziToast.error({
                title: 'Error',
                message: 'No se pudo inactivar el registro, contacte al administrador',
            });
        }
        else {
            // REFRESCAR UNICAMENTE LA TABLA
            
            cargarGridComisiones();
            console.log("Crago grid");
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#InactivarEmpleadoComisiones").modal('hide');
            $("#EditarEmpleadoComisiones").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue Inactivado de forma exitosa!',
            });
        }
    });
});