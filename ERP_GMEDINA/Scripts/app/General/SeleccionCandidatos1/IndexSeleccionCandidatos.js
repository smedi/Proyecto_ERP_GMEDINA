var id = 0;
//Funciones GET
function tablaEditar(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().id;
    _ajax(null,
        '/SeleccionCandidatos/Edit/' + id,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#FormEditar").find("#per_Id").val(obj.per_Id);
                $("#FormEditar").find("#fare_Id").val(obj.fare_Id);
                $("#FormEditar").find("#scan_Fecha").val(obj.scan_Fecha);
                $("#FormEditar").find("#rper_Id").val(obj.rper_Id);
                $('#ModalEditar').modal('show');
            }
        });
}
function tablaDetalles(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().id;
    _ajax(null,
        '/SeleccionCandidatos/Edit/' + id,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#ModalDetalles").find("#per_Id")["0"].innerText = obj.per_Id;
                $("#ModalDetalles").find("#fare_Id")["0"].innerText = obj.fare_Id;
                $("#ModalDetalles").find("#scan_Fecha")["0"].innerText = obj.scan_Fecha;
                $("#ModalDetalles").find("#rper_Id")["0"].innerText = obj.rper_Id;
                $("#ModalDetalles").find("#scan_Estado")["0"].innerText = obj.scan_Estado;
                $("#ModalDetalles").find("#scan_RazonInactivo")["0"].innerText = obj.scan_RazonInactivo;
                $("#ModalDetalles").find("#scan_FechaCrea")["0"].innerText = FechaFormato(obj.scan_FechaCrea);
                $("#ModalDetalles").find("#scan_FechaModifica")["0"].innerText = FechaFormato(obj.scan_FechaModifica);
                $("#ModalDetalles").find("#tbUsuario_usu_NombreUsuario")["0"].innerText = obj.tbUsuario.usu_NombreUsuario;
                $("#ModalDetalles").find("#tbUsuario1_usu_NombreUsuario")["0"].innerText = obj.tbUsuario1.usu_NombreUsuario;
                $("#ModalDetalles").find("#btnEditar")["0"].dataset.id = id;
                $('#ModalDetalles').modal('show');
            }
        });
}
function llenarTabla() {
    _ajax(null,
        '/SeleccionCandidatos/llenarTabla',
        'POST',
        function (Lista) {
            tabla.clear();
            tabla.draw();
            $.each(Lista, function (index, value) {
                tabla.row.add({
                    id: value.scan_Id,
                    d: value.per_Id,
                    Id: value.fare_Id,
                    Fecha: value.scan_Fecha,
                    Id: value.rper_Id
                });
            });
            tabla.draw();
        });
}
$(document).ready(function () {
    llenarTabla();
});
//Botones GET
$("#btnAgregar").click(function () {
    var modalnuevo = $('#ModalNuevo');
    modalnuevo.modal('show');
    $(modalnuevo).find("#per_Id").val("");
    $(modalnuevo).find("#per_Id").focus();
    $(modalnuevo).find("#fare_Id").val("");
    $(modalnuevo).find("#scan_Fecha").val("");
    $(modalnuevo).find("#rper_Id").val("");
});
$("#btnEditar").click(function () {
    _ajax(null,
        '/SeleccionCandidatos/Edit/' + id,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                CierraPopups();
                $('#ModalEditar').modal('show');
                $("#ModalEditar").find("#per_Id").val(obj.per_Id);
                $("#ModalEditar").find("#per_Id").focus();
                $("#ModalEditar").find("#fare_Id").val(obj.fare_Id);
                $("#ModalEditar").find("#scan_Fecha").val(obj.scan_Fecha);
                $("#ModalEditar").find("#rper_Id").val(obj.rper_Id);
            }
        });
});
$("#btnInhabilitar").click(function () {
    CierraPopups();
    $('#ModalInhabilitar').modal('show');
    $("#ModalInhabilitar").find("#scan_RazonInactivo").val("");
    $("#ModalInhabilitar").find("#scan_RazonInactivo").focus();
});
//botones POST
$("#btnGuardar").click(function () {
    var data = $("#FormNuevo").serializeArray();
    data = serializar(data);
    if (data != null) {
        data = JSON.stringify({ tbSeleccionCandidatos: data });
        _ajax(data,
            '/SeleccionCandidatos/Create',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    LimpiarControles(["per_Id", "fare_Id", "scan_Fecha", "rper_Id", "scan_RazonInactivo"]);
                    MsgSuccess("¡Exito!", "Se ah agregado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});
$("#InActivar").click(function () {
    var data = $("#FormInactivar").serializeArray();
    data = serializar(data);
    if (data != null) {
        data.scan_Id = id;
        data = JSON.stringify({ tbSeleccionCandidatos: data });
        _ajax(data,
            '/SeleccionCandidatos/Delete',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    LimpiarControles(["per_Id", "fare_Id", "scan_Fecha", "rper_Id", "scan_RazonInactivo"]);
                    MsgWarning("¡Exito!", "Se ah Inactivado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});
$("#btnActualizar").click(function () {
    var data = $("#FormEditar").serializeArray();
    data = serializar(data);
    if (data != null) {
        data.scan_Id = id;
        data = JSON.stringify({ tbSeleccionCandidatos: data });
        _ajax(data,
            '/SeleccionCandidatos/Edit',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    MsgSuccess("¡Exito!", "Se ah actualizado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});