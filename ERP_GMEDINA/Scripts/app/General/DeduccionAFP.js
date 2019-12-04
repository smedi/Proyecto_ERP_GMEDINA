//
//OBTENER SCRIPT DE FORMATEO DE FECHA
//
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
function cargarGridDeducciones() {
    _ajax(null,
        '/DeduccionAFP/GetData',
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
            var ListaDeduccion = data, template = '';
            //RECORRER DATA OBETINA Y CREAR UN "TEMPLATE" PARA REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            for (var i = 0; i < ListaDeduccionAFP.length; i++) {
                template += '<tr data-id = "' + ListaDeduccionAFP[i].dafp_Id + '">' +
                    '<td>' + ListaDeduccionAFP[i].per_Nombres + ' ' + ListaDeduccion[i].per_Apellidos + '</td>' +
                    '<td>' + ListaDeduccionAFP[i].emp_CuentaBancaria + '</td>' +
                    '<td>' + ListaDeduccionAFP[i].dafp_AporteLps + '</td>' +
                    '<td>' + ListaDeduccionAFP[i].afp_Descripcion + '</td>' +
                    '<td>' +
                    '<button type="button" data-id = "' + ListaDeduccionAFP[i].dafp_Id + '" class="btn btn-primary btn-xs" id="btnEditarDeduccionAFP">Editar</button>' +
                    '<button type="button" data-id = "' + ListaDeduccionAFP[i].dafp_Id + '" class="btn btn-default btn-xs" id="btnDetalleDeduccionAFP">Detalle</button>' +
                    '</td>' +
                    '</tr>';
            }
            //REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            $('#tbodyDeducciones').html(template);
        });
}

//Editar//
//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$(document).on("click", "#tblDeduccionAFP tbody tr td #btnEditarDeduccionAFP", function () {
    var ID = $(this).data('id');
    $.ajax({
        url: "/DeduccionAFP/Edit/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })

        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                console.log('Hla')
                $("#Editar #dafp_AporteLps").val(data.dafp_AporteLps);
                //GUARDAR EL ID DEL DROPDOWNLIST (QUE ESTA EN EL REGISTRO SELECCIONADO) QUE NECESITAREMOS PONER SELECTED EN EL DDL DEL MODAL DE EDICION


                var SelectedIdEmpleado = data.emp_Id;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST EMPLEADO PARA EL MODAL
                $.ajax({
                    url: "/DeduccionAFP/EditGetEmpleadoDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Editar #per_Nombres + #per_Apellidos").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Editar #per_Nombres + #per_Apellidos").append("<option value = 0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Editar #per_Nombres + #per_Apellidos").append("<option" + (iter.Id == SelectedIdEmpleado ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });


                var SelectedIdAFP = data.afp_Id;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST AFP PARA EL MODAL
                $.ajax({
                    url: "/DeduccionAFP/EditGetAFPDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Editar #afp_Descripcion").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Editar #afp_Descripcion").append("<option value = 0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Editar #afp_Descripcion").append("<option" + (iter.Id == SelectedIdAFP ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });



                $("#EditarDeduccionAFP").modal();

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

//EJECUTAR EDICIÓN DEL REGISTRO EN EL MODAL
$("#btnEditDeduccionAFP").click(function () {
    //SERIALIZAR EL FORMULARIO (QUE ESTÁ EN LA VISTA PARCIAL) DEL MODAL, SE PARSEA A FORMATO JSON
    var data = $("#frmEditDeduccionAFP").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/DeduccionAFP/Edit",
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
            cargarGridDeducciones();
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#EditarDeduccionAFP").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue editado de forma exitosa!',
            });
        }
    });
});

//FUNCION: OCULTAR MODAL DE EDICIÓN
$("#btnCerrarEditar").click(function () {
    $("#EditarDeduccionAFP").modal('hide');
});




//Agregar//
//FUNCION: PRIMERA FASE DE AGREGAR UN NUEVO REGISTRO, MOSTRAR MODAL DE CREATE
$(document).on("click", "#btnAgregarDeduccionAFP", function () {

    //CARGAR INFORMACIÓN DEL DROPDOWNLIST EMPLEADO PARA EL MODAL
    $.ajax({
        url: "/DeduccionAFP/EditGetEmpleadoDDL",
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID })
    })
        .done(function (data) {
            //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
            $("#Editar #per_Nombres + #per_Apellidos").empty();
            //LLENAR EL DROPDOWNLIST
            $("#Editar #per_Nombres + #per_Apellidos").append("<option value = 0>Selecione una opción...</option>");
            $.each(data, function (i, iter) {
                $("#Editar #per_Nombres + #per_Apellidos").append("<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
            });
        });

    //CARGAR INFORMACIÓN DEL DROPDOWNLIST AFP PARA EL MODAL
    $.ajax({
        url: "/DeduccionAFP/EditGetAFPDDL",
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID })
    })
        .done(function (data) {
            //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
            $("#Editar #afp_Descripcion").empty();
            //LLENAR EL DROPDOWNLIST
            $("#Editar #afp_Descripcion").append("<option value = 0>Selecione una opción...</option>");
            $.each(data, function (i, iter) {
                $("#Editar #afp_Descripcion").append("<optionvalue='" + iter.Id + "'>" + iter.Descripcion + "</option>");
            });
        });


    //MOSTRAR EL MODAL DE AGREGAR
    $("#AgregarCatalogoDeducciones").modal();
});


//FUNCION: CREAR EL NUEVO REGISTRO
$('#btnCreateRegistroDeduccionAFP').click(function () {
    // SIEMPRE HACER LAS RESPECTIVAS VALIDACIONES DEL LADO DEL CLIENTE

    //SERIALIZAR EL FORMULARIO DEL MODAL (ESTÁ EN LA VISTA PARCIAL)
    var data = $("#frmCreateDeduccionAFP").serializeArray();
    //ENVIAR DATA AL SERVIDOR PARA EJECUTAR LA INSERCIÓN
    $.ajax({
        url: "/DeduccionAFP/Create",
        method: "POST",
        data: data
    }).done(function (data) {
        //CERRAR EL MODAL DE AGREGAR
        $("#AgregarDeduccionAFP").modal('hide');
        //VALIDAR RESPUESTA OBETNIDA DEL SERVIDOR, SI LA INSERCIÓN FUE EXITOSA O HUBO ALGÚN ERROR
        if (data == "error") {
            iziToast.error({
                title: 'Error',
                message: 'No se pudo guardar el registro, contacte al administrador',
            });
        }
        else {
            cargarGridDeducciones();
            // Mensaje de exito cuando un registro se ha guardado bien
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue registrado de forma exitosa!',
            });
        }
    });
});




//Detalles//
///////////////////////////////////////////////////////////////////////////////////////////////////
$(document).on("click", "#tblDeduccionAFP tbody tr td #btnDetalleDeduccionAFP", function () {
    var ID = $(this).data('id');
    $.ajax({
        url: "/DeduccionAFP/Details/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })
        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                var FechaCrea = FechaFormato(data.dafp_FechaCrea);
                var FechaModifica = FechaFormato(data.dafp_FechaModifica);
                $("#Detalles #emp_Id").val(data.emp_Id);
                $("#Detalles #per_Nombres + #per_Apellidos").val(data.per_Nombres + data.per_Apellidos);
                $("#Detalles #emp_CuentaBancaria").val(data.emp_CuentaBancaria);
                $("#Detalles #dafp_AporteLps").val(data.dafp_AporteLps);
                $("#Detalles #afp_Id").val(data.afp_Id);
                $("#Detalles #afp_Descripcion").val(data.afp_Descripcion);
                $("#Detalles #dafp_UsuarioCrea").val(data.dafp_UsuarioCrea);
                $("#Detalles #dafp_FechaCrea").val(FechaCrea);
                $("#Detalles #dafp_UsuarioModifica").val(data.dafp_UsuarioModifica);
                $("#Detalles #dafp_FechaModifica").val(FechaModifica);

                var SelectedIdEmpleado = data.emp_Id;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST EMPLEADO PARA EL MODAL
                $.ajax({
                    url: "/DeduccionAFP/EditGetEmpleadoDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Detalles #per_Nombres + #per_Apellidos").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Detalles #per_Nombres + #per_Apellidos").append("<option value = 0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Detalles #per_Nombres + #per_Apellidos").append("<option" + (iter.Id == SelectedIdEmpleado ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });


                var SelectedIdAFP = data.afp_Id;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST AFP PARA EL MODAL
                $.ajax({
                    url: "/DeduccionAFP/EditGetAFPDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Detalles #afp_Descripcion").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Detalles #afp_Descripcion").append("<option value = 0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Detalles #afp_Descripcion").append("<option" + (iter.Id == SelectedIdAFP ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });

                $("#DetallesDeduccionAFP").modal();

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


$(document).on("click", "#btnmodalInactivarCatalogoDeducciones", function () {
    //MOSTRAR EL MODAL DE INACTIVAR
    $("#InactivarDeduccionAFP").modal();
});

//EJECUTAR INACTIVACION DEL REGISTRO EN EL MODAL
$("#btnInactivarRegistroDeduccionAFP").click(function () {

    var data = $("#frmDeduccionAFPInactivar").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/DeduccionAFP/Inactivar",
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
            cargarGridDeducciones();
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#InactivarDeduccionAFP").modal('hide');
            $("#EditarDeduccionAFP").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue Inactivado de forma exitosa!',
            });
        }
    });
});


// PROBANDO LOS IZITOAST
//$(document).ready(function () {
//    console.log('cargado JS');
//    iziToast.show({
//        title: 'Hola',
//        message: 'Estoy probando los iziToast'
//    });
//});


