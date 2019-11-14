//VARIABLE GLOBAL PARA INACTIVAR

var inactivar = 0;

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
function cargarGridTipoDeducciones() {
    _ajax(null,
        '/TipoDeducciones/GetData',
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
            var ListaTipoDeducciones = data, template = '';
            //RECORRER DATA OBETINA Y CREAR UN "TEMPLATE" PARA REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            for (var i = 0; i < ListaTipoDeducciones.length; i++) {
                template += '<tr data-id = "' + ListaTipoDeducciones[i].tde_IdTipoDedu + '">' +
                    '<td>' + ListaTipoDeducciones[i].tde_Descripcion + '</td>' +
                    '<td>' +
                    '<button type="button" class="btn btn-default btn-xs" id="btnEditarTipoDeducciones">Editar</button>'+
                    '<button type="button" class="btn btn-primary btn-xs" id="btnDetalleTipoDeduccion">Detalle</button>' +
                    '</td>' +
                    '</tr>';
            }
            //REFRESCAR EL TBODY DE LA TABLA DEL INDEX
            $('#tbodyTipoDeducciones').html(template);
        });
}

//FUNCION: PRIMERA FASE DE AGREGAR UN NUEVO REGISTRO, MOSTRAR MODAL DE CREATE
$(document).on("click", "#btnAgregarTipoDeducciones", function () {
    //PEDIR DATA PARA LLENAR EL DROPDOWNLIST DEL MODAL
    //$.ajax({
    //    url: "/TipoDeducciones/EditGetDDL",
    //    method: "GET",
    //    dataType: "json",
    //    contentType: "application/json; charset=utf-8"
    //})
    //LLENAR EL DROPDONWLIST DEL MODAL CON LA DATA OBTENIDA
    //.done(function (data) {
    //    $("#Crear #tde_IdTipoDedu").empty();
    //    $("#Crear #tde_IdTipoDedu").append("<option value='0'>Selecione una opción...</option>");
    //    $.each(data, function (i, iter) {
    //        $("#Crear #tde_IdTipoDedu").append("<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>");
    //    });
    //});

    //MOSTRAR EL MODAL DE AGREGAR
    $("#AgregarTipoDeducciones").modal();
});

//FUNCION: CREAR EL NUEVO REGISTRO
$('#btnCreateRegistroTipoDeducciones').click(function(){
    // SIEMPRE HACER LAS RESPECTIVAS VALIDACIONES DEL LADO DEL CLIENTE
    
    //SERIALIZAR EL FORMULARIO DEL MODAL (ESTÁ EN LA VISTA PARCIAL)
    var data = $("#frmTipoDeduccionCreate").serializeArray();
    //ENVIAR DATA AL SERVIDOR PARA EJECUTAR LA INSERCIÓN
    $.ajax({
        url: "/TipoDeducciones/Create",
        method: "POST",
        data: data
    }).done(function (data) {
        //CERRAR EL MODAL DE AGREGAR
        $("#AgregarTipoDeducciones").modal('hide');
        //VALIDAR RESPUESTA OBETNIDA DEL SERVIDOR, SI LA INSERCIÓN FUE EXITOSA O HUBO ALGÚN ERROR
        if (data == "error") {
            iziToast.error({
                title: 'Error',
                message: 'No se pudo guardar el registro, contacte al administrador',
            });
        }
        else {
            cargarGridTipoDeducciones();
            // Mensaje de exito cuando un registro se ha guardado bien
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue registrado de forma exitosa!',
            });
        }
    });
});

//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$(document).on("click", "#tblTipoDeducciones tbody tr td #btnEditarTipoDeducciones", function () {
    
    var ID = $(this).closest('tr').data('id');
    inactivar = ID;
    console.log(ID);
    $.ajax({
        url: "/TipoDeducciones/Edit/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    })
        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                console.log(data);
                //debugger;
                $.each(data, function (i, iter) {
                    $("#Editar #tde_IdTipoDedu").val(iter.tde_IdTipoDedu);
                    $("#Editar #tde_Descripcion").val(iter.tde_Descripcion);
                });
                $("#EditarTipoDeducciones").modal();
                cargarGridTipoDeducciones();
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
$("#btnUpdateTipoDeducciones").click(function () {
    //SERIALIZAR EL FORMULARIO (QUE ESTÁ EN LA VISTA PARCIAL) DEL MODAL, SE PARSEA A FORMATO JSON
    var data = $("#frmTipoDeduccionEdit").serializeArray();
    //SE ENVIA EL JSON AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    $.ajax({
        url: "/TipoDeducciones/Edit",
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
            cargarGridTipoDeducciones();
            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
            $("#EditarTipoDeducciones").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue editado de forma exitosa!',
            });
        }
    });
});

//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$("#btnInactivarTipoDeducciones").click(function () {
    console.log(inactivar);
    $.ajax({
        url: "/TipoDeducciones/Inactivar/" + inactivar,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: inactivar })
    })
        .done(function (data) {
            //SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
            if (data) {
                console.log(data);
                //debugger;
                $.each(data, function (i, iter) {
                    $("#Editar #tde_IdTipoDedu").val(iter.tde_IdTipoDedu);                
                    $("#Editar #tde_UsuarioModifica").val(iter.tde_UsuarioModifica);
                    $("#Editar #tde_FechaModifica").val(iter.tde_FechaModifica);
                });
                $("#EditarTipoDeducciones").modal();
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

//FUNCION: OCULTAR MODAL DE EDICIÓN
$("#btnCerrarEditarTipoDeducciones").click(function () {
    $("#EditarTipoDeducciones").modal('hide');
});