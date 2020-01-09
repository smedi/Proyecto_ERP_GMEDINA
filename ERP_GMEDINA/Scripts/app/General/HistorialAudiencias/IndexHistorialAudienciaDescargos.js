


function format(obj) {


    var div = '<div class="ibox"><div class="ibox-title"><h5>Audiencias de Descargo</h5> <div align=right><button type="button" class="btn btn-primary btn-xs" onclick="Llamarmodalcreate(' + idEmpleado + ')">Registrar</button> </div> </div><div class="ibox-content"><div class="row">'
        + '<table class="table table-striped table-bordered table-hover dataTables-example" >'
        + '<thead>'
        + '<tr> <th>  Motivo  </th>'
        + '<th>Fecha</th>'
        + '<th>Testigo</th> '
         + '<th>Acciones</th> '
        + '</tr> </thead> ';
    obj.forEach(function (index, value) {
        div = div +
            '<tbody>' + '<tr>'
                + '<td>' + index.aude_Descripcion + '</td>'
                + '<td>' + FechaFormato(index.aude_FechaAudiencia).substring(0,10) + '</td>'
                + '<td>' + index.aude_Testigo + '</td>'
               
                + '<td>' + '<button type="button" class="btn btn-danger btn-xs" onclick="Llamarmodaldelete(' + index.aude_Id + ')" data-id="@item.cin_IdIngreso">Inactivar</button> <button type="button" class="btn btn-default btn-xs" onclick="Llamarmodaldetalle(' + index.aude_Id + ')" data-id="@item.cin_IdIngreso">Detalle</button>' + '</td>'
                + '</tr>' + '</tbody>'
        '</table>'


    });
    return div + '</div></div>';
}





function llenarTabla() {
    _ajax(null,
       '/HistorialAudienciaDescargos/llenarTabla',
       'POST',
       function (Lista) {
           tabla.clear();
           tabla.draw();
           $.each(Lista, function (index, value) {
               tabla.row.add({
                   Id: value.emp_Id,
                   Empleado: value.Empleado,
                   Cargo: value.Cargo,
                   Departamento: value.Departamento
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
        idEmpleado = id
        hola = row.data().hola;
        _ajax({ id: parseInt(id) },
            '/HistorialAudienciaDescargos/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.removeClass('loading');
                    tr.addClass('shown');
                }
            });
    }
});



function Llamarmodaldetalle(ID) {
    debugger
    var modalnuevo = $("#ModalDetalles");
    _ajax({ ID: parseInt(ID) },
        '/HistorialAudienciaDescargos/Edit/',
        'GET',
        function (obj) {

            if (obj != "-1" && obj != "-2" && obj != "-3") {
                //$("#ModalDetalles").find("#emp_Id")["0"].innerText = obj.NombreCompleto;
                $("#ModalDetalles").find("#aude_Descripcion")["0"].innerText = obj.aude_Descripcion;
                $("#ModalDetalles").find("#aude_FechaAudiencia")["0"].innerText = FechaFormato(obj.aude_FechaAudiencia).substring(0,10);
                $("#ModalDetalles").find("#aude_Testigo")["0"].innerText = obj.aude_Testigo;
                $("#ModalDetalles").find("#aude_DireccionArchivo")["0"].innerText = obj.aude_DireccionArchivo;
                $("#ModalDetalles").find("#tbUsuario_usu_NombreUsuario")["0"].innerText = obj.tbUsuario;
                $("#ModalDetalles").find("#aude_FechaCrea")["0"].innerText = FechaFormato(obj.aude_FechaCrea).substring(0, 10);
                //$("#ModalDetalles").find("#hinc_UsuarioCrea")["0"].innerText = obj.hinc_UsuarioCrea;
                //$("#ModalDetalles").find("#hinc_UsuarioModifica")["0"].innerText = obj.hinc_UsuarioModifica;
                //$("#ModalDetalles").find("#hinc_FechaModifica")["0"].innerText = FechaFormato(obj.hinc_FechaModifica).substring(0, 10);
                $('#ModalDetalles').modal('show');

            }
        });

}

function Llamarmodalcreate() {

    var modalnuevo = $("#ModalNuevo");
    $("#ModalNuevo").find("#emp_Id").val(idEmpleado);
    modalnuevo.modal('show');
}



$("#btnGuardar").click(function () {
    debugger
    var data = $("#FormNuevo").serializeArray();
    data = serializar(data);
    if (data != null) {
        data = JSON.stringify({ tbHistorialAudienciaDescargo: data });
        _ajax(data,
            '/HistorialAudienciaDescargos/Create',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    LimpiarControles(["emp_Id", "aude_Descripcion", "aude_FechaAudiencia", "aude_Testigo", "aude_DireccionArchivo"]);
                    MsgSuccess("¡Exito!", "Se agrego el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});



function Llamarmodaldelete(ID) {

    var modalnuevo = $("#ModalInhabilitar");
    $("#ModalInhabilitar").find("#aude_Id").val(ID);
    modalnuevo.modal('show');
}


$("#InActivar").click(function () {
    var data = $("#FormInactivar").serializeArray();
    data = serializar(data);
    debugger
    if (data != null) {
        data = JSON.stringify({ tbHistorialAudienciaDescargo: data });
        _ajax(data,
            '/HistorialAudienciaDescargos/Delete',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    debugger
                    llenarTabla();
                    LimpiarControles(["aude_Id", "aude_RazonInactivo"]);
                    MsgWarning("¡Exito!", "Se inactivo el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});

