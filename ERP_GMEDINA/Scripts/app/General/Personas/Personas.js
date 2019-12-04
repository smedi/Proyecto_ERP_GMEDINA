var id = 0;
function tablaDetalles(ID) {
    id = ID;
    _ajax(null,
        '/Personas/Edit/' + ID,
        'GET',
        function (obj) {
            if (obj != "-1" && obj != "-2" && obj != "-3") {
                $("#ModalDetalles").find("#per_Identidad")["0"].innerText = obj.per_Identidad;
                $("#ModalDetalles").find("#per_Nombres")["0"].innerText = obj.per_Nombres;
                $("#ModalDetalles").find("#per_Apellidos")["0"].innerText = obj.per_Apellidos;
                $("#ModalDetalles").find("#per_Sexo")["0"].innerText = obj.per_Sexo;
                $("#ModalDetalles").find("#per_Edad")["0"].innerText = obj.per_Edad;
                $("#ModalDetalles").find("#per_Direccion")["0"].innerText = obj.per_Direccion;
                $("#ModalDetalles").find("#per_Estado")["0"].innerText = obj.per_Estado;
                $("#ModalDetalles").find("#per_RazonInactivo")["0"].innerText = obj.per_RazonInactivo;
                $("#ModalDetalles").find("#per_FechaCrea")["0"].innerText = FechaFormato(obj.per_FechaCrea);
                $("#ModalDetalles").find("#per_FechaModifica")["0"].innerText = FechaFormato(obj.per_FechaModifica);
                $("#ModalDetalles").find("#tbUsuario_usu_NombreUsuario")["0"].innerText = obj.tbUsuario.usu_NombreUsuario;
                $("#ModalDetalles").find("#tbUsuario1_usu_NombreUsuario")["0"].innerText = obj.tbUsuario1.usu_NombreUsuario;
                $("#ModalDetalles").find("#btnEditar")["0"].dataset.id = ID;
                $('#ModalDetalles').modal('show');
            }
        });
}