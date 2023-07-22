
function AbrirModal() {
    $("#ModalUpdate").modal("show");
}
function CerrarModal() {
    $('#ModalUpdate').modal('hide')
    IniciarUsuario();
}
function GetById(IdUsuario) {
    $.ajax({
        type: 'GET',
        url: 'http://localhost:5145/api/Usuario/MostrarUsuarioId/' + IdUsuario,
        success: function (result) {
            $('#txtIdUsuario').val(result.objeto.idUsuario);
            $('#txtNombre').val(result.objeto.nombre);
            $('#txtApellidos').val(result.objeto.apellidos);
            $('#txtEdad').val(result.objeto.edad);
            $('#txtEmail').val(result.objeto.email);
            $('#ModalUpdate').modal('show');
        },
        error: function (result) {
            alert('Error en la consulta.' + result.responseJSON.mensajeError);
        }
    });
}
function IniciarUsuario() {
    var usuario = {
        idUsuario: $('#txtIdUsuario').val(''),
        nombre: $('#txtNombre').val(''),
        apellidos: $('#txtApellidos').val(''),
        edad: $('#txtEdad').val(''),
        email: $('#txtEmail').val(''),
    }
}

