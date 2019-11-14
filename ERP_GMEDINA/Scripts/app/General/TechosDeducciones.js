﻿////FUNCION GENERICA PARA REUTILIZAR AJAX
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


var InactivarID = 0;
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

//FUNCION: CARGAR DATA Y REFRESCAR LA TABLA DEL INDEX
function cargarGridTechosDeducciones() {
    _ajax(null,
        '/TechosDeducciones/GetData',
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
            var ListaTechosDeducciones = data, template = '';
            //RECORRER DATA OBETINA Y CREAR UN "TEMPLATE" PARA REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            for (var i = 0; i < ListaTechosDeducciones.length; i++) {
                template += '<tr data-id = "' + ListaTechosDeducciones[i].tede_Id + '">' +
                    '<td>' + ListaTechosDeducciones[i].tede_RangoInicial + '</td>' +
                    '<td>' + ListaTechosDeducciones[i].tede_RangoFinal + '</td>' +
                    '<td>' + ListaTechosDeducciones[i].tede_Porcentaje + '</td>' +
                    '<td>' + ListaTechosDeducciones[i].cde_DescripcionDeduccion + '</td>' +
                    '<td>' +
                    '<button type="button" class="btn btn-primary btn-xs" data-id = "' + ListaTechosDeducciones[i].tede_Id + '" id="btnEditarTechosDeducciones">Editar</button>' +
                    '<button type="button" class="btn btn-default btn-xs" data-id = "' + ListaTechosDeducciones[i].tede_Id + '" id="btnDetalleTechosDeducciones">Detalle</button>' +
                    '</td>' +
                    '</tr>';
            }
            //REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            $('#tbodyTechosDeducciones').html(template);
        });
}

//Modal Create Techos Deducciones
$(document).on("click", "#btnAgregarTechosDeducciones", function () {
    //PEDIR DATA PARA LLENAR EL DROPDOWNLIST DEL MODAL
    $.ajax({
        url: "/TechosDeducciones/EditGetDDL",
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    })
        //LLENAR EL DROPDONWLIST DEL MODAL CON LA DATA OBTENIDA
        .done(function (data) {
            $("#Crear #cde_IdDeducciones").empty();
            $("#Crear #cde_IdDeducciones").append("<option value='0'>Selecione una opción...</option>");
            $.each(data, function (i, iter) {
                $("#Crear #cde_IdDeducciones").append("<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
            });
        });
    //MOSTRAR EL MODAL DE AGREGAR
    $("#AgregarTechosDeducciones").modal();
});

//FUNCION: CREAR EL NUEVO REGISTRO TECHOS DEDUCCIONES
$('#btnCreateTechoDeducciones').click(function () {
    // SIEMPRE HACER LAS RESPECTIVAS VALIDACIONES DEL LADO DEL CLIENTE

    //SERIALIZAR EL FORMULARIO DEL MODAL (ESTÁ EN LA VISTA PARCIAL)
    var data = $("#frmTechosDeduccionesCreate").serializeArray();
    //ENVIAR DATA AL SERVIDOR PARA EJECUTAR LA INSERCIÓN
    $.ajax({
        url: "/TechosDeducciones/Create",
        method: "POST",
        data: data
    }).done(function (data) {
        //CERRAR EL MODAL DE AGREGAR
        $("#AgregarTechosDeducciones").modal('hide');
        //VALIDAR RESPUESTA OBETNIDA DEL SERVIDOR, SI LA INSERCIÓN FUE EXITOSA O HUBO ALGÚN ERROR
        if (data == "error") {
            iziToast.error({
                title: 'Error',
                message: 'No se pudo guardar el registro, contacte al administrador',
            });
        }
        else if (data == "bien") {                        
            cargarGridTechosDeducciones();
            console.log(data);
            // Mensaje de exito cuando un registro se ha guardado bien
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue registrado de forma exitosa!',
            });
        }
    });
});

//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$(document).on("click", "#tblTechosDeducciones tbody tr td #btnEditarTechosDeducciones", function () {
    $("#EditarTechosDeducciones").modal();
    var ID = $(this).data('id');
    InactivarID = ID;
    $.ajax({
        url: "/TechosDeducciones/Edit/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })

        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                $("#Editar #tede_Id").val(data.tede_Id);
                $("#Editar #tede_RangoInicial").val(data.tede_RangoInicial);
                $("#Editar #tede_RangoFinal").val(data.tede_RangoFinal);
                $("#Editar #tede_Porcentaje").val(data.tede_Porcentaje);
                $("#Editar #cde_IdDeduccion").val(data.cde_IdDeducciones);
                //GUARDAR EL ID DEL DROPDOWNLIST (QUE ESTA EN EL REGISTRO SELECCIONADO) QUE NECESITAREMOS PONER SELECTED EN EL DDL DEL MODAL DE EDICION
                var SelectedId = data.cde_IdDeducciones;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST PARA EL MODAL
                $.ajax({
                    url: "/TechosDeducciones/EditGetDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Editar #cde_IdDeducciones").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Editar #cde_IdDeducciones").append("<option value=0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Editar #cde_IdDeducciones").append("<option" + (iter.Id == SelectedId ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });
                //$("#EditarTechosDeducciones").modal();
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
$("#btnEditarTecho").click(function () {
    //SERIALIZAR EL FORMULARIO (QUE ESTÁ EN LA VISTA PARCIAL) DEL MODAL, SE PARSEA A FORMATO JSON
    var data = $("#frmEditTechosDeducciones").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/TechosDeducciones/Edit",
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
            cargarGridTechosDeducciones();
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#EditarTechosDeducciones").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Éxito',
                message: '¡El registro fue editado de forma exitosa!',
            });
        }
    });
});

//FUNCION: OCULTAR MODAL DE EDICIÓN
$("#btnCerrarEditar").click(function () {
    $("#EditarTechosDeducciones").modal('hide');
});


////Inactivar
//$(document).on("click", "#tblTechosDeducciones tbody tr td #btnInactivarTechosDeducciones", function () {
//    InactivarID = $(this).data('id');
//    //$.ajax({
//    //    url: "/TechosDeducciones/Inactivar/" + ID,
//    //    method: "GET",
//    //    dataType: "json",
//    //    contentType: "application/json; charset=utf-8",
//    //    data: JSON.stringify({ ID: ID })
//    //})
//    $("#InactivarTechosDeducciones").modal();
//})


//EJECUTAR EDICIÓN DEL REGISTRO EN EL MODAL
$("#btnInactivarTechoDeducciones").click(function () {    
    $.ajax({
        url: "/TechosDeducciones/Inactivar/" + InactivarID,
        method: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: InactivarID })
    })
        .done(function (data) {
            if (data == "error") {
                //Cuando traiga un error del backend al guardar la edicion
                iziToast.error({
                    title: 'Error',
                    message: 'No se pudo editar el registro, contacte al administrador',
                });
            }
            else {
                // REFRESCAR UNICAMENTE LA TABLA
                cargarGridTechosDeducciones();
                //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
                $("#EditarTechosDeducciones").modal('hide');
                //Mensaje de exito de la edicion
                iziToast.success({
                    title: 'Éxito',
                    message: '¡El registro fue inactivado de forma exitosa!',
                });
            }
        });
    InactivarID = 0;
});

$(document).on("click", "#tblTechosDeducciones tbody tr td #btnDetalleTechosDeducciones", function () {
    var ID = $(this).data('id');
    $.ajax({
        url: "/TechosDeducciones/Details/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })
        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {                
                var FechaCrea = FechaFormato(data.tede_FechaCrea);
                var FechaModifica = FechaFormato(data.tede_FechaModifica);
                $("#Detalles #tede_Id").val(data.tede_Id);
                $("#Detalles #tede_RangoInicial").val(data.tede_RangoInicial);
                $("#Detalles #tede_RangoFinal").val(data.tede_RangoFinal);
                $("#Detalles #tede_Porcentaje").val(data.tede_Porcentaje);
                $("#Detalles #cde_IdDeducciones").val(data.cde_IdDeducciones);
                $("#Detalles #tede_UsuarioCrea").val(data.tede_UsuarioCrea);
                $("#Detalles #tede_FechaCrea").val(FechaCrea);
                $("#Detalles #tede_UsuarioModifica").val(data.tede_UsuarioModifica);
                $("#Detalles #tede_FechaModifica").val(FechaModifica);
                //GUARDAR EL ID DEL DROPDOWNLIST (QUE ESTA EN EL REGISTRO SELECCIONADO) QUE NECESITAREMOS PONER SELECTED EN EL DDL DEL MODAL DE EDICION
                var SelectedId = data.cde_IdDeducciones;
                //CARGAR INFORMACIÓN DEL DROPDOWNLIST PARA EL MODAL
                $.ajax({
                    url: "/TechosDeducciones/EditGetDDL",
                    method: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ ID })
                })
                    .done(function (data) {
                        //LIMPIAR EL DROPDOWNLIST ANTES DE VOLVER A LLENARLO
                        $("#Detalles #cde_IdDeducciones").empty();
                        //LLENAR EL DROPDOWNLIST
                        $("#Detalles #cde_IdDeducciones").append("<option value=0>Selecione una opción...</option>");
                        $.each(data, function (i, iter) {
                            $("#Detalles #cde_IdDeducciones").append("<option" + (iter.Id == SelectedId ? " selected" : " ") + " value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
                        });
                    });
                $("#DetailsTechosDeducciones").modal();
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