function format(obj) {
    var div = '<div class="col-lg-12  >';
    var Id = "";
    var Comp = "";
    var Hab = "";
    var ReEs = "";
    var Idi = "";
    var Tit = "";
    obj.forEach(function (index, value) {
        Id = index.per_Id.toString();
        if (index.Relacion == "1")
        {
            Comp += index.Descripcion.toString() + '<br>';
            
        }
        else if (index.Relacion == "2")
        {
            Hab +=  index.Descripcion.toString() + '<br>' ;
        }
        else if(index.Relacion == "3")
        {
            Idi +=  index.Descripcion.toString() + '<br>' ;
        }
        else if (index.Relacion == "4")
        {
            ReEs += index.Descripcion.toString() + '<br>' ;
        }
        else if (index.Relacion)
        {
            Tit += index.Descripcion.toString() + '<br>';
        }
    });
    div += '<div class="row col-lg-12">';
    ////Habilidades
    //div += '<div class="col-lg-4 ">';
    //div += '<div class="ibox">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Habilidades </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += ''+ Hab +'';
    //div += '</form>';
    //div += '</div></div></div>';
    //Habilidades
    div += '<div class="ibox">';
    div += '<div class="ibox-content" style="">';
    div += '<div class="panel-body">';
    div += '<div class="panel-group" id="accordion">';
    div += '<div class="panel panel-default">';
    div += '<div class="panel-heading">';
    div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#Competencias' + Id + '" class="collapsed" aria-expanded="false">Competencias</a></h5>';
    div += '</div>';
    div += '<div id="Competencias' + Id + '" class="panel-collapse in collapse" style="">';
    div += '<div class="panel-body">';
    div += '' + Comp + '';
    div += '</div>';
    div += '</div>';
    div += '</div>';

    //
    div += '<div class="panel panel-default">';
    div += '<div class="panel-heading" data-toggle="collapse" data-parent="#accordion" href="#Habilidades" class="" aria-expanded="true">';
    div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#Habilidades" class="collapsed" aria-expanded="false">Habilidades</a></h5>';
    div += '</div>';
    div += '<div id="Habilidades" class="panel-collapse in collapse" style="">';
    div += '<div class="panel-body">';
    div += '' + Hab + '';
    div += '</div>';
    div += '</div>';
    div += '</div>';

    div += '<div class="panel panel-default">';
    div += '<div class="panel-heading">';
    div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#Titulos" class="collapsed" aria-expanded="false">Titulos</a></h5>';
    div += '</div>';
    div += '<div id="Titulos" class="panel-collapse in collapse" style="">';
    div += '<div class="panel-body">';
    div += '' + Tit + '';
    div += '</div>';
    div += '</div>';
    div += '</div>';

    div += '<div class="panel panel-default">';
    div += '<div class="panel-heading">';
    div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#R_Especiales" class="collapsed" aria-expanded="false">Requerimientos Especiales</a></h5>';
    div += '</div>';
    div += '<div id="R_Especiales" class="panel-collapse in collapse" style="">';
    div += '<div class="panel-body">';
    div += '' + ReEs + '';
    div += '</div>';
    div += '</div>';
    div += '</div>';

    div += '<div class="panel panel-default">';
    div += '<div class="panel-heading">';
    div += '<h5 class="panel-title"><a data-toggle="collapse" data-parent="#accordion" href="#Idiomas" class="collapsed" aria-expanded="false">Idiomas</a></h5>';
    div += '</div>';
    div += '<div id="Idiomas" class="panel-collapse in collapse" style="">';
    div += '<div class="panel-body">';
    div += '' + Idi + '';
    div += '</div>';
    div += '</div>';
    div += '</div>';

    div += '</div>';
    div += '</div>';
    //



    ////Habilidades
    //div += '<div class="col-lg-4 margin-2">';
    //div += '<div class="ibox">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Habilidades </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += ''+ Hab +'';
    //div += '</form>';
    //div += '</div></div></div>';

    ////Competencias
    //div += '<div class="col-lg-4">';
    //div += '<div class="ibox ">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Competencias </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += '' + Comp + '';
    //div += '</form>';
    //div += '</div></div></div>';
    ////Idiomas
    //div += '<div class="col-lg-4">'
    //div += '<div class="ibox ">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Idiomas </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += '' + Idi + '';
    //div += '</form>';
    //div += '</div></div></div>';
    //div += '</div>';
    ////Segunda Linea
    //div += '<div class="row">';
    //div += '<div class="col-lg-2"></div>';
    ////Requerimientos Epeciales
    //div += '<div class="col-lg-4">'
    //div += '<div class="ibox ">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Requerimientos Epeciales </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += '' + ReEs + '';
    //div += '</form>';
    //div += '</div></div></div>';
    ////Titulos
    //div += '<div class="col-lg-4">';
    //div += '<div class="ibox ">';
    //div += '<div class="ibox-title">';
    //div += '<h5> Titulos </h5>';
    //div += '<div class="ibox-tools">';
    //div += '<a class="collapse-link"> <i class="fa fa-chevron-up"></i> </a>';
    //div += '</div> </div>';
    //div += '<div class="ibox-content">'
    //div += '<form role="form" class="form-inline">';
    //div += '' + Tit + '';
    //div += '</form>';
    //div += '</div></div></div>';
    //div += '<div class="col-lg-2"></div>';
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
