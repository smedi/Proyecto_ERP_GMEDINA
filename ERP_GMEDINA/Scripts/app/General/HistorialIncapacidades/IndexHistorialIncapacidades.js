

function format(obj) {

    
    var div = '<div class="ibox"><div class="ibox-title"><h5>Incapacidades</h5> <div align=right><button type="button" class="btn btn-primary btn-xs" onclick="Llamarmodalcreate(' + idEmpleado + ')" data-id="@item.cin_IdIngreso">Nueva Incapacidad</button> </div> </div><div class="ibox-content"><div class="row">'
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
                    + '<td>' + FechaFormato(index.hinc_FechaInicio).substring(0, 10) + '</td>'
                    + '<td>' + FechaFormato(index.hinc_FechaFin).substring(0, 10) + '</td>'
                    + '<td>' + '<button type="button" class="btn btn-danger btn-xs" onclick="Llamarmodaldelete()" data-id="@item.cin_IdIngreso">Inactivar</button> <button type="button" class="btn btn-default btn-xs" onclick="Llamarmodaldetalle()" data-id="@item.cin_IdIngreso">Detalle</button>' + '</td>'
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

var idEmpleado = 0;

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

function Llamarmodalcreate() {

    var modalnuevo = $("#ModalNuevo");
    $("#ModalNuevo").find("#id")["0"].innerText = idEmpleado;

    modalnuevo.modal('show');


}



function tablaDetalles(ID) {
    id = ID;
    _ajax(null,
        '/HistorialIncapacidades/Details/' + ID,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#ModalDetalles").find("#hinc_Descripcion")["0"].innerText = obj.hinc_Descripcion;
                $("#ModalDetalles").find("#ticn_Descripcion")["0"].innerText = obj.ticn_Descripcion;
                $("#ModalDetalles").find("#hinc_Dias")["0"].innerText = obj.hinc_Dias;
                $("#ModalDetalles").find("#hinc_CentroMedico")["0"].innerText = FechaFormato(obj.hinc_CentroMedico);
                $("#ModalDetalles").find("#hinc_Doctor")["0"].innerText = FechaFormato(obj.hinc_Doctor);
                $("#ModalDetalles").find("#hinc_Diagnostico")["0"].innerText = obj.hinc_Diagnostico;
                $("#ModalDetalles").find("#hinc_FechaInicio")["0"].innerText = obj.hinc_FechaInicio;
                $("#ModalDetalles").find("#hinc_FechaFin")["0"].innerText = obj.hinc_FechaFin;
                $('#ModalDetalles').modal('show');
            }
        });
}



function llenarDropDownList() {
    _ajax(null,
       '/HistorialIncapacidades/llenarDropDowlist',
       'POST',
       function (result) {
           $.each(result, function (id, Lista) {
               Lista.forEach(function (value, index) {
                   $("#" + id).append(new Option(value.Descripcion, value.Id));
               });
           });
       });
}




