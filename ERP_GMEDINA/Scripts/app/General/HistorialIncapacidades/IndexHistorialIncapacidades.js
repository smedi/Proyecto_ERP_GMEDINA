function format(obj) {  
    var div = '<div class="ibox"><div class="ibox-title"><h5>Incapacidades</h5> <div align=right><button type="button" class="btn btn-primary btn-xs" onclick="LlamarmodalNuevo()" data-id="@item.cin_IdIngreso">Nueva Incapacidad</button> </div> </div><div class="ibox-content"><div class="row">'
        +'<table class="table table-striped table-bordered table-hover dataTables-example" >'
        +'<thead>'
        +'<tr> <th>  Incapacidad  </th>'
        +'<th>Dias de retiro</th>'
        + '<th>Centro Medico</th> '
         + '<th>Diagnostico</th> '
         + '<th>Fecha Inicio</th> '
         + '<th>Fecha Fin</th> '
         + '<th>Acciones</th> '
        +'</tr> </thead> ';
    obj.forEach(function (index, value) {
        div = div +
            '<tbody>' + '<tr>'
                + '<td>' + index.ticn_Descripcion + '</td>'
                + '<td>' + index.hinc_Dias + '</td>'
                + '<td>' + index.hinc_CentroMedico + '</td>'
                + '<td>' + index.hinc_Diagnostico + '</td>'
                + '<td>' + FechaFormato(index.hinc_FechaInicio).substring(0,10) + '</td>'
                + '<td>' + FechaFormato(index.hinc_FechaFin).substring(0,10) + '</td>'
                + '<td>' + '<button type="button" class="btn btn-danger btn-xs" onclick="Llamarmodaldelete()" data-id="@item.cin_IdIngreso">Eliminar</button> <button type="button" class="btn btn-default btn-xs" onclick="Llamarmodaldetalle()" data-id="@item.cin_IdIngreso">Detalle</button>' + '</td>'
                + '</tr>' + '</tbody>'
            '</table>'
    });
    return div + '</div></div>';
}
function llenarTabla() {
    _ajax(null,
       '/HistorialIncapacidades/llenarTabla',
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
        hola = row.data().hola;
        _ajax({ id: parseInt(id) },
            '/HistorialIncapacidades/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.addClass('shown');
                }
            });
    }
});


function Llamarmodaldelete() {

    var modalnuevo = $("#ModalInhabilitar");
    modalnuevo.modal('show');


}

function Llamarmodaldetalle() {

    var modalnuevo = $("#ModalDetalles");
    modalnuevo.modal('show');


}