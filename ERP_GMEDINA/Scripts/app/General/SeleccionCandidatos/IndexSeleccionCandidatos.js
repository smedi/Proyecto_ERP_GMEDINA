function tablaDetalles(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().Id;
    $(location).attr('href', "/SeleccionCandidatos/Edit/" + id);
}
function tablaEditar(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().Id;
    $(location).attr('href', "/SeleccionCandidatos/Edit/" + id);
}

//LLENAR INDEX 
function llenarTabla() {
    _ajax(null,
       '/SeleccionCandidatos/llenarTabla',
       'POST',
       function (Lista) {
           tabla.clear();
           tabla.draw();
           $.each(Lista, function (index, value) {
               tabla.row.add({
                   Id: value.Id,
                   Identidad: value.Identidad,
                   Nombre: value.Nombre,
                   Fase: value.Fase,
                   Plaza_Solicitada: value.Plaza_Solicitada,
                   Fecha: FechaFormato(value.Fecha).substring(0, FechaFormato(value.Fecha).length - 8)
               });
           });
           tabla.draw();
       });
}
$(document).ready(function () {
    llenarTabla();
});
$('#IndexTable tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = tabla.row(tr);



    if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        id = row.data().Id;
        hola = row.data().hola;
        _ajax({ id: parseInt(id) },
            '/SeleccionCandidatos/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.addClass('shown');
                }
            });
    }
});





//LLAMAR MODALES 
function CallEditar() {
    var modalnuevo = $("#ModalEditar");
    modalnuevo.modal('show');
}

function CallDetalles() {
    var modalnuevo = $("#ModalDetalles");
    modalnuevo.modal('show');
}

function btnAgregar() {
    var modalnuevo = $("#ModalNuevo");
    modalnuevo.modal('show');
}

function btnInactivar() {
    var modalnuevo = $("#ModalInactivar");
    modalnuevo.modal('show');
}

function btnContratar() {
    var modalnuevo = $("#ModalContratar");
    modalnuevo.modal('show');
}

$("#btnGuardar").click(function () {
    var data = $("#FormNuevo").serializeArray();
    data = serializar(data);
    
    if (data != null) {
        debugger
        data = JSON.stringify({ tbSeleccionCandidatos: data });
        debugger
        _ajax(data,
            '/SeleccionCandidatos/Create',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    LimpiarControles(["per_Id", "fare_Id", "scan_Fecha"]);
                    MsgSuccess("¡Exito!", "Se ah agregado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});

//DETALLES
function tablaDetalles(ID) {
    _ajax(null,
        '/SeleccionCandidatos/Edit/' + ID,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#ModalDetalles").find("#Identidad")["0"].innerText = obj.Identidad;
                $("#ModalDetalles").find("#Nombre")["0"].innerText = obj.Nombre;
                $("#ModalDetalles").find("#Fase")["0"].innerText = obj.Fase;
                $("#ModalDetalles").find("#Plaza_Solicitada")["0"].innerText = obj.Plaza_Solicitada;
                $("#ModalDetalles").find("#Fecha")["0"].innerText = obj.Fecha;
                //$("#ModalDetalles").find("#ticn_RazonInactivo")["0"].innerText = obj.ticn_RazonInactivo;
                //$("#ModalDetalles").find("#ticn_FechaCrea")["0"].innerText = obj.FechaCrea;
                //$("#ModalDetalles").find("#ticn_FechaModifica")["0"].innerText = obj.FechaModifica;
                //$("#ModalDetalles").find("#tbUsuario_usu_NombreUsuario")["0"].innerText = obj.tbUsuario.usu_NombreUsuario;
                //$("#ModalDetalles").find("#tbUsuario1_usu_NombreUsuario")["0"].innerText = obj.tbUsuario1.usu_NombreUsuario;
                $("#ModalDetalles").find("#btnEditar")["0"].dataset.id = ID;
                $('#ModalDetalles').modal('show');
            }
        });
}
