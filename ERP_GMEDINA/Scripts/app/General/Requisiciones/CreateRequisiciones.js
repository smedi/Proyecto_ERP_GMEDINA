
function ListFill(obj) {
    var SlctCompetencias = $(".SlctCompetencias");
    var SlctHabilidades = $(".SlctHabilidades");
    var SlctIdiomas = $(".SlctIdiomas");
    var SlctReqEspeciales = $(".SlctReqEspeciales");
    var SlctTitulos = $(".SlctTitulos");
    obj.Competencias.forEach(function (index, value) {
        SlctCompetencias.append('<option value="' + index.comp_Id + '">' + index.comp_Descripcion + '</option>');
    });

    obj.Habilidades.forEach(function (index, value) {
        SlctHabilidades.append('<option value="' + index.habi_Id + '">' + index.habi_Descripcion + '</option>');
    });

    obj.Idiomas.forEach(function (index, value) {
        SlctIdiomas.append('<option value="' + index.idi_Id + '">' + index.idi_Descripcion + '</option>');
    });

    obj.ReqEspeciales.forEach(function (index, value) {
        SlctReqEspeciales.append('<option value="' + index.resp_Id + '">' + index.resp_Descripcion + '</option>');
    });

    obj.Titulos.forEach(function (index, value) {
        SlctTitulos.append('<option value="' + index.titu_Id + '">' + index.titu_Descripcion + '</option>');
    });

    SlctCompetencias.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctHabilidades.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctIdiomas.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctReqEspeciales.bootstrapDualListbox({ selectorMinimalHeight: 160 });
    SlctTitulos.bootstrapDualListbox({ selectorMinimalHeight: 160 });
};

$(document).ready(function () {
    _ajax(null,
            '/Requisiciones/DualListBoxData',
            'GET',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    ListFill(obj);
                }
            })

    id = sessionStorage.getItem("IdPersona");
    _ajax(null,
        '/Personas/Edit/' + id,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                var fecha = FechaFormatoSimple(obj[0].per_FechaNacimiento);
                console.log(fecha);
                $("#tbPersonas").find("#per_Id").val(obj[0].per_Id);
                $("#tbPersonas").find("#per_Identidad").val(obj[0].per_Identidad);
                $("#tbPersonas").find("#per_Nombres").val(obj[0].per_Nombres);
                $("#tbPersonas").find("#per_Apellidos").val(obj[0].per_Apellidos);
                $("#tbPersonas").find("#per_FechaNacimiento").val(FechaFormatoSimple(obj[0].per_FechaNacimiento));
                $("#tbPersonas").find("#per_Sexo").val(obj[0].per_Sexo);
                $("#tbPersonas").find("#per_Edad").val(obj[0].per_Edad);
                $("#tbPersonas").find("#nac_Id").val(obj[0].nac_Id);
                $("#tbPersonas").find("#per_Direccion").val(obj[0].per_Direccion);
                $("#tbPersonas").find("#per_Telefono").val(obj[0].per_Telefono);
                $("#tbPersonas").find("#per_CorreoElectronico").val(obj[0].per_CorreoElectronico);
                $("#tbPersonas").find("#per_EstadoCivil").val(obj[0].per_EstadoCivil);
                $("#tbPersonas").find("#per_TipoSangre").val(obj[0].per_TipoSangre);
            }
        });

    var wizard = $("#Wizard").steps({
        enableCancelButton: false,
        onFinished: function () {
            var SlctCompetencias = $(".SlctCompetencias");
            var SlctHabilidades = $(".SlctHabilidades");
            var SlctIdiomas = $(".SlctIdiomas");
            var SlctReqEspeciales = $(".SlctReqEspeciales");
            var SlctTitulos = $(".SlctTitulos");
            
            var data = { Competencias: SlctCompetencias.val(), Habilidades: SlctHabilidades.val(), Idiomas: SlctIdiomas.val(), ReqEspeciales: SlctReqEspeciales.val(), Titulos: SlctTitulos.val() };
            var Form = $("#tbRequisiciones").find("select, textarea, input:not(:hidden)").serializeArray();
            Form = serializarPro(Form);
            Form = JSON.stringify({ tbRequisiciones: Form, DatosProfesionales: data });
            console.log(Form);

            _ajax(Form,
            '/Requisiciones/Create',
            'POST',
            function (obj) {
                if (obj != "-1" && obj != "-2" && obj != "-3") {
                    MsgSuccess("¡Exito!", "Se ah agregado el registro");
                } else {
                    MsgError("Error", "Codigo:" + obj + ". contacte al administrador.(Verifique si el registro ya existe)");
                }
            });
        },
    });
});

//, Habilidades: SlctHabilidades.val(), Idiomas: SlctIdiomas, ReqEspeciales: SlctReqEspeciales.val(), Titulos : SlctTitulos.val()