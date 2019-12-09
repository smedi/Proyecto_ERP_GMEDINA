﻿function format(obj) {
var div = '<div class="ibox"><div class="ibox-title"><h5>Incapacidades</h5></div><div class="ibox-content"><div class="row"> <table class="table table-striped table-bordered table-hover dataTables-example" > <thead> <tr> <th>  Incapacidad  </th> <th>Dias de retiro</th>  <th>Centro Medico</th>     </tr> </thead> ';
    obj.forEach(function (index, value) {
        div = div +
            '<tbody>' + '<tr>'
                + '<td>' + index.ticn_Descripcion + '</td>'
                + '<td>' + index.hinc_Dias + '</td>'
                + '<td>' + index.hinc_CentroMedico + '</td>' 
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