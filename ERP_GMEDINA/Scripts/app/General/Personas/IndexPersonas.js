function format(obj) {
    var div = '<div class="col-lg-12  >';
    div += '<div class="row col-lg-12">';
    div += '<div class="ibox">';
    div += '<div class="ibox-content" style="">';
    div += '<div class="panel-body">';
    div += '<div class="panel-group" id="accordion">';
    var Id = "";   var Comp = ""   ;var Hab = ""   ;var ReEs = "";   var Idi = "";   var Tit = "";   var Id="";
    obj.forEach(function (index, value) {
        debugger
        Id = index.per_Id.toString();
        if(index.Relacion == "Competencias") 
            Comp += index.Descripcion.toString() + '<br>';
        else if(index.Relacion == "Habilidades")
            Hab +=  index.Descripcion.toString() + '<br>' ;
        else if(index.Relacion == "Idiomas")
            Idi += index.Descripcion.toString() + '<br>';
        else if (index.Relacion == "Requerimientos_Especiales")
            ReEs += index.Descripcion.toString() + '<br>' ;
        else if (index.Relacion == "Titulos")
            Tit += index.Descripcion.toString() + '<br>';
    });
    if (Comp.length == 0)
        Comp += 'Sin Datos.';
    if (Hab.length == 0)
        Hab += 'Sin Datos.';
    if (Idi.length == 0)
        Idi += 'Sin Datos.';
    if (ReEs.length == 0)
        ReEs += 'Sin Datos.';
    if (Tit.length == 0)
        Tit += 'Sin Datos.';
    var TodoPersona = [Comp, Hab, Idi, ReEs, Tit];
    var Encabezados = ['Competencias', 'Habilidades', 'Idiomas', 'Requerimientos_Especiales', 'Titulos'];
    for (i = 0 ; i < TodoPersona.length ; i++) {
        Encabezados[i]
        div += '<div class="panel panel-default">';
        div += '<div class="panel-heading" data-toggle="collapse" data-parent="#accordion" href="#' + Encabezados[i] + Id + '" class="collapsed" aria-expanded="false">';
        div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#' + Encabezados[i] + Id + '" class="collapsed" aria-expanded="false">' + Encabezados[i] + '</a></h5>';
        div += '</div>';
        div += '<div id="' + Encabezados[i] + Id + '" class="panel-collapse in collapse" style="">';
        div += '<div class="panel-body">';
        div += '' + TodoPersona[i] + '';
        div += '</div>';
        div += '</div>';
        div += '</div>';
    }
    Id = "";
    div += '</div>';
    return div ;
}
function llenarTabla() {
    _ajax(null,
       '/Personas/llenarTabla',
       'POST',
       function (Lista) {
           tabla.clear();
           tabla.draw();
           $.each(Lista, function (index, value) {
               tabla.row.add({
                   Id: value.Id,
                   Identidad: value.Identidad,
                   NombreCompleto: value.Nombre,//length == 0 ? 'Desconocido' : value.NombreCompleto[0],
                   //Sexo: value.Sexo,
                   //Direccion: value.Direccion,
                   //Nacionalidad: value.Nacionalidad,
                   CorreoElectronico: value.CorreoElectronico,
                   //Telefono: value.Telefono //.length == 0 ? 'Desconocido' : value.per_Telefono[0]
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
            '/Personas/ChildRowData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    row.child(format(obj)).show();
                    tr.addClass('shown');
                }
            });
    }


});
