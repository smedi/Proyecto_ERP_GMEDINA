﻿function tablaDetalles(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().Id;
    $(location).attr('href', "/Areas/details/" + id);
}
function tablaEditar(btn) {
    var tr = $(btn).closest("tr");
    var row = tabla.row(tr);
    id = row.data().Id;
    $(location).attr('href', "/Areas/Edit/" + id);
}
function format(obj) {
    var div = '<div class="ibox">' +
                '<div class="ibox-title"><h5>Departamentos</h5>' +
                '<div class="ibox-tools">' +
                        '<a class="collapse-link" onclick="flecha(this)" >' +
                        '<i class="fa fa-chevron-up"></i>' +
                        '</a>' +
                '</div>' +
                '</div><div class="ibox-content"><div class="row">';
    obj.forEach(function (index,value) {
        div = div +
            '<div class="col-md-3">'+
                '<div class="ibox">' +
                  '<div class="ibox-title">' +
                     '<h5>' + index.depto_Descripcion + '</h5>' +                     
                '</div>'+
                '<div class="ibox-content">' +
                    '<h5>' + index.car_Descripcion + '</h5>'
                    //'<span class="fa fa-user-o m-r-xs"></span>' +
                    + index.per_NombreCompleto + '<br>' +
                    //'<span class="fa fa-phone m-r-xs"></span>' +
                    index.per_Telefono + '</div>' +
                '</div>'+
            '</div>'
    });
    return div + '</div></div></div>';
}
function llenarTabla() {
    _ajax(null,
       '/Areas/llenarTabla',
       'POST',
       function (Lista) {
           tabla.clear();
           tabla.draw();
           $.each(Lista, function (index, value) {
               tabla.row.add({
                   Id: value.area_Id,
                   Descripcion: value.area_Descripcion,
                   Encargado: value.Encargado.length == 0 ? 'Sin Asignar' : value.Encargado[0]
               });
           });
           tabla.draw();
       });
}
function flecha(obj) {
    var ibox = $(obj).closest('div.ibox');
    var button = $(obj).find('i');
    var content = ibox.find('div.ibox-content');
    content.slideToggle(200);
    button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
    ibox.toggleClass('').toggleClass('border-bottom');
    setTimeout(function () {
        ibox.resize();
        ibox.find('[id^=map-]').resize();
    }, 50);
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
            '/Areas/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.addClass('shown');
                }
            });
    }
});