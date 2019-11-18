var IDInactivar = 0;
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

//FUNCION: PRIMERA FASE DE EDICION DE REGISTROS, MOSTRAR MODAL CON LA INFORMACIÓN DEL REGISTRO SELECCIONADO
$(document).on("click", "#tblFormaDePago tbody tr td #btnDetalleFormaDePago", function () {
    var ID = $(this).closest('tr').data('id');
    IDInactivar = ID;
    $.ajax({
        url: "/FormaPago/Details/" + ID,
        method: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ID: ID })
    }).done(function (data) {
        debugger;
        console.log("String message");
        ////SI SE OBTIENE DATA, LLENAR LOS CAMPOS DEL MODAL CON ELLA
        if (data) {

            $.each(data, function (i, iter) {
                var FechaCrea = FechaFormato(iter.fpa_FechaCrea);
                var FechaModifica = FechaFormato(iter.fpa_FechaModifica);
                $("#Detallar #fpa_Descripcion").val(iter.fpa_Descripcion);
                iter.fpa_Activo == true ? $("#Detallar #fpa_Activo").attr('checked', true) : '';
                $("#Detallar #fpa_UsuarioCrea").val(iter.fpa_UsuarioCrea);
                $("#Detallar #fpa_FechaCrea").val(FechaCrea);
                $("#Detallar #fpa_UsuarioModifica").val(iter.fpa_UsuarioModifica);
                $("#Detallar #fpa_FechaModifica").val(FechaModifica);
            });

        }
        $("#DetalleFormaPago").modal();
    });
});

//Boton para cerrar el modal de Detalles
$("#btnCerrarDetails").click(function () {
    $("#DetalleFormaPago").modal('hide');
});

//Boton para mostrar el modal de confirmacion para inactivar
$("#btnInactivar").click(function () {
    $("#DetalleFormaPago").modal('hide');
    $("#InactivarFormaPago").modal();
});

//Boton para realiazar la inactivacion del registro
$("#btnConfirmarInactivacion").click(function () {
    //SE REALIZA LA PETICION AL SERVIDOR PARA EJECUTAR LA EDICIÓN
    //var data = $("#frmFormaDePago").serializeArray();
    console.log("ID Inactivacion: " + IDInactivar);
    $.ajax({
        url: "/FormaPago/Inactivar/" + IDInactivar,
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

            //UNA VEZ REFRESCADA LA TABLA, SE OCULTA EL MODAL
                $("#InactivarFormaPago").modal('hide');
            //Mensaje de exito de la edicion
            iziToast.success({
                title: 'Exito',
                message: 'El registro fue editado de forma exitosa!',
            });
        }
    });
    IDInactivar = 0;
});

//Boton para cerrar el modal de Inactivar
$("#btnCerrarInactivacion").click(function () {
    $("#InactivarFormaPago").modal('hide');
});






