function format(obj) {
    var div = '<div class="ibox"><div class="ibox-title"><h5>Vacaciones</h5><div align=right> <button type="button" class="btn btn-primary btn-xs" onclick="llamarmodal(' + IdEmpleado + ')">Agregar Vacaciones</button> </div></div><div class="ibox-content"><div class="row">' + '<table class="table table-striped table-borderef table-hover dataTables-example"> ' +
        '<thead>' +
            '<tr>' +
                '<th>' + 'Fecha inicio' + '</th>' +
                '<th>' + 'Fecha fin' + '</th>' +
                '<th>' + 'Cantidad dias' + '</th>' +
                '<th>' + 'Mes vacaciones' + '</th>' +
                '<th>' + 'Año vacaciones' + '</th>' +
                '<th>' + 'Acciones' + '</th>' +
                '</tr>' +
                '</thead>';
    obj.forEach(function (index, value) {
        div = div +
                '<tbody>' +
                '<tr>' +
                '<td>' + FechaFormato(index.hvac_FechaInicio).substring(0, 10) + '</td>' +
                '<td>' + FechaFormato(index.hvac_FechaFin).substring(0, 10) + '</td>' +
                '<td>' + index.hvac_CantDias + '</td>' +
                '<td>' + index.hvac_MesVacaciones + '</td>' +
                '<td>' + index.hvac_AnioVacaciones + '</td>' +
                '<td>' + ' <button type="button" class="btn btn-danger btn-xs" onclick="llamarmodaldelete(' + IdEmpleado + ')" data-id="@item.hamo_Id">Inactivar</button> <button type="button" class="btn btn-default btn-xs" onclick="llamarmodaldetalles(' + IdEmpleado + ')"data-id="@item.hamo_Id">Detalle</button>' + '</td>' +
                '</tr>' +
                '</tbody>' 
                '</table>'
         
    });
    return div + '</div></div>';
}
function llenarTabla() {
    _ajax(null,
       '/HistorialVacaciones/llenarTabla',
       'POST',
       function (Lista) {
           tabla.clear();
           tabla.draw();
           $.each(Lista, function (index, value) {
               tabla.row.add({
                   Id: value.emp_Id,
                   Empleados: value.Empleado,
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
var IdEmpleado = 0;
$('#IndexTable tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = tabla.row(tr);
    if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        id = row.data().Id;
        IdEmpleado = id;
        hola = row.data().hola;
        _ajax({ id: parseInt(id) },
            '/HistorialVacaciones/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.addClass('shown');
                }
            });
    }
});


function llamarmodal() {
    var modalnuevo = $("#ModalNuevo");
    $("#ModalNuevo").find("#emp_Id").val(IdEmpleado);
    modalnuevo.modal('show');
}
function llamarmodaldelete() {
    var modaldelete = $("#ModalInhabilitar");
    modaldelete.modal('show');
}

function llamarmodaldetalles() {
    var modaldetalle = $("#ModalDetalles");
    id = IdEmpleado;
    debugger
    _ajax({id: parseInt(id)},
        '/HistorialVacaciones/Edit',
        'GET',
        function (obj) {
            $('#ModalDetalles').modal('show');
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#ModalDetalles").find("#hvac_FechaInicio")["0"].innerText = obj.hvac_FechaInicio;
                $("#ModalDetalles").find("#hvac_FechaFin")["0"].innerText = obj.hvac_FechaFin;
                $("#ModalDetalles").find("#hvac_CantDias")["0"].innerText = obj.hvac_CantDias;
                $("#ModalDetalles").find("#hvac_DiasPagados")["0"].innerText = obj.hvac_DiasPagados;
                $("#ModalDetalles").find("#hvac_MesVacaciones")["0"].innerText = obj.hvac_MesVacaciones;
                $("#ModalDetalles").find("#hvac_AnioVacaciones")["0"].innerText = obj.hvac_AnioVacaciones;
                $("#ModalDetalles").find("#tbUsuario_usu_NombreUsuario")["0"].innerText = obj.tbUsuario.usu_NombreUsuario;
                $("#ModalDetalles").find("#hvac_FechaCrea")["0"].innerText = obj.hvac_FechaCrea;
                $("#ModalDetalles").find("#tbUsuario1_usu_NombreUsuario")["0"].innerText = obj.tbUsuario1.usu_NombreUsuario;
                $("#ModalDetalles").find("#hvac_FechaModifica")["0"].innerText = obj.hvac_FechaModifica;
                debugger
                
            }
        });
}

//------DROPDOWLIST-----------

function llenarDropDownList() {
    _ajax(null,
       '/HistorialAmonestaciones/llenarDropDowlist',
       'POST',
       function (result) {
           $.each(result, function (id, Lista) {
               Lista.forEach(function (value, index) {
                   $("#" + id).append(new Option(value.Descripcion, value.Id));
               });
           });
       });
}
function Remove(Id, lista) {
    var list = [];
    lista.forEach(function (value, index) {
        if (value.Id != Id) {
            list.push(value);
        }
    });
    return list;
}
function llenarDropDownList() {
    _ajax(null,
       '/HistorialAmonestaciones/llenarDropDowlist',
       'POST',
       function (result) {
           $.each(result, function (id, Lista) {
               Lista.forEach(function (value, index) {
                   $("#" + id).append(new Option(value.Descripcion, value.Id));
               });
           });
       });
}
$("#btnGuardar").click(function () {
    var data = $("#FormNuevo").serializeArray();
    data = serializar(data);
    debugger
    if (data != null) {
        data = JSON.stringify({ tbHistorialVacaciones: data });
        _ajax(data,
            '/HistorialVacaciones/Create',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    CierraPopups();
                    llenarTabla();
                    LimpiarControles(["emp_Id", "hvac_FechaInicio", "hvac_FechaFin"]);
                    MsgSuccess("¡Exito!", "Se ah agregado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
    } else {
        MsgError("Error", "por favor llene todas las cajas de texto");
    }
});


