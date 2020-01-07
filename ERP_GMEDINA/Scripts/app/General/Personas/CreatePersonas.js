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

    SlctCompetencias.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctHabilidades.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctIdiomas.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctReqEspeciales.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctTitulos.bootstrapDualListbox({ selectorMinimalHeight: 160 });
};
$(document).ready(function () {
    llenarDropDownList();
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

            var DatosProfesionalesArray = { Competencias: SlctCompetencias.val(), Habilidades: SlctHabilidades.val(), Idiomas: SlctIdiomas.val(), ReqEspeciales: SlctReqEspeciales.val(), Titulos: SlctTitulos.val() };
            var Form = $("#tbPersonas").find("select, textarea, input").serializeArray();
            tbPersonas = serializar(Form);
            data = JSON.stringify({ tbPersonas, DatosProfesionalesArray });
            console.log(data);

            if (tbPersonas != null)
            {
                _ajax(data,
                '/Personas/Create',
                'POST',
                function (obj) {
                    if (obj != "-1" && obj != "-2" && obj != "-3") {
                        MsgSuccess("¡Exito!", "Se ah agregado el registro");
                        window.location.href = "Index";
                    } else {
                        MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                    }
                });
            }
            else {
                MsgError("Error", "por favor llene todos los datos de texto");
            }
        },
    });
});