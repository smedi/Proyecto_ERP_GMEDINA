$(document).ready(function () {


    $("#Candidato").val(sessionStorage.getItem("per_Descripcion"));
    llenarDropDownList()
});

function llenarDropDownList() {
    _ajax(null,
       '/SeleccionCandidatos/llenarDropDowlist',
       'POST',
       function (result) {
           $.each(result, function (id, Lista) {

               debugger
               Lista.forEach(function (value, index) {
                   $("#" + id).append(new Option(value.Descripcion, value.Id));
               });
           });
       });
}