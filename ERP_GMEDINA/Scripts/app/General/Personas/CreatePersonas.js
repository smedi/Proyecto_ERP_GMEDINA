function llenarDropDownList() {
    _ajax(null,
       '/Personas/llenarDropDowlistNacionalidades',
       'POST',
       function (result) {
           $.each(result, function (id, Lista) {
               Lista.forEach(function (value, index) {
                   $("#nac_Id" ).append('<option value="' + value.Id + '">' + value.Descripcion + '</option>');
               });
           });
       });
}
function ListFill(obj) {
    var SlctCompetencias = $(".SlctCompetencias");
    var SlctHabilidades = $(".SlctHabilidades");
    var SlctIdiomas = $(".SlctIdiomas");
    var SlctReqEspeciales = $(".SlctReqEspeciales");
    var SlctTitulos = $(".SlctTitulos");
    obj.forEach(function (index, value) {
        if (index.TipoDato == "C") {
                SlctCompetencias.append('<option value="' + index.Id + '">' + index.Descripcion + '</option>');
        }
        if (index.TipoDato == "H") {
                SlctHabilidades.append('<option value="' + index.Id + '">' + index.Descripcion + '</option>');
        }
        if (index.TipoDato == "I") {
                SlctIdiomas.append('<option value="' + index.Id + '">' + index.Descripcion + '</option>');
        }
        if (index.TipoDato == "R") {
                SlctReqEspeciales.append('<option value="' + index.Id + '">' + index.Descripcion + '</option>');
        }
        if (index.TipoDato == "T") {
                SlctTitulos.append('<option value="' + index.Id + '">' + index.Descripcion + '</option>');
        }
    });

    SlctCompetencias.bootstrapDualListbox({ selectorMinimalHeight: 160, filterPlaceHolder: 'Buscar...', infoText: 'Mostrando {0}', infoTextEmpty: 'Lista vacia', infoTextFiltered: '<span class="label label-warning">Coincidencias</span> {0} de {1}', filterTextClear: 'Mostrar todos', moveAllLabel : 'Mover todos',removeAllLabel: 'Remover todos'});
    SlctHabilidades.bootstrapDualListbox({ selectorMinimalHeight: 160, filterPlaceHolder: 'Buscar...', infoText: 'Mostrando {0}', infoTextEmpty: 'Lista vacia', infoTextFiltered: '<span class="label label-warning">Coincidencias</span> {0} de {1}', filterTextClear: 'Mostrar todos', moveAllLabel: 'Mover todos', removeAllLabel: 'Remover todos' });
    SlctIdiomas.bootstrapDualListbox({ selectorMinimalHeight: 160, filterPlaceHolder: 'Buscar...', infoText: 'Mostrando {0}', infoTextEmpty: 'Lista vacia', infoTextFiltered: '<span class="label label-warning">Coincidencias</span> {0} de {1}', filterTextClear: 'Mostrar todos', moveAllLabel: 'Mover todos', removeAllLabel: 'Remover todos' });
    SlctReqEspeciales.bootstrapDualListbox({ selectorMinimalHeight: 160, filterPlaceHolder: 'Buscar...', infoText: 'Mostrando {0}', infoTextEmpty: 'Lista vacia', infoTextFiltered: '<span class="label label-warning">Coincidencias</span> {0} de {1}', filterTextClear: 'Mostrar todos', moveAllLabel: 'Mover todos', removeAllLabel: 'Remover todos' });
    SlctTitulos.bootstrapDualListbox({ selectorMinimalHeight: 160, filterPlaceHolder: 'Buscar...', infoText: 'Mostrando {0}', infoTextEmpty: 'Lista vacia', infoTextFiltered: '<span class="label label-warning">Coincidencias</span> {0} de {1}', filterTextClear: 'Mostrar todos', moveAllLabel: 'Mover todos', removeAllLabel: 'Remover todos' });
};
$(document).ready(function () {
    llenarDropDownList();
    $("#per_FechaNacimiento").attr("max", Mayor18());
    _ajax(null,
            '/Personas/DualListBoxData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    ListFill(obj);
                }
            })

    var wizard = $("#Wizard").steps({
        enableCancelButton: false,
        onFinished: function () {
            var SlctCompetencias = $(".SlctCompetencias");
            var SlctHabilidades = $(".SlctHabilidades");
            var SlctIdiomas = $(".SlctIdiomas");
            var SlctReqEspeciales = $(".SlctReqEspeciales");
            var SlctTitulos = $(".SlctTitulos");
            var Correo = validarEmail($('#per_CorreoElectronico').val());
            var Nombre = validarNombre($('#per_Nombres').val(), $('#per_Apellidos').val());

            var DatosProfesionalesArray = { Competencias: SlctCompetencias.val(), Habilidades: SlctHabilidades.val(), Idiomas: SlctIdiomas.val(), ReqEspeciales: SlctReqEspeciales.val(), Titulos: SlctTitulos.val() };
            var Form = $("#tbPersonas").find("select, textarea, input").serializeArray();
            tbPersonas = serializar(Form);
            data = JSON.stringify({ tbPersonas, DatosProfesionalesArray });

            //
            if (tbPersonas != null)
            {
                if (Nombre != " ")
                {
                    if (Correo != " ") {
                        _ajax(data,
                        '/Personas/Create',
                        'POST',
                        function (obj) {
                            if (obj != "-1" && obj != "-2" && obj != "-3") {
                                MsgSuccess("¡Exito!", "Se ah agregado el registro");
                                $("#finish").attr("href", " ");
                                setTimeout(function () { window.location.href = "/Personas/Index"; }, 5000);
                            } else {
                                MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                            }
                        });
                    }
                    else {
                        MsgError("Error", "Correo Electronico invalido");
                    }
                }
                else {
                    MsgError("Error", "Nombres o Apellidos no validos");
                }
            }
            else {
                MsgError("Error", "por favor llene todos los datos");
            }
        },
    });
});