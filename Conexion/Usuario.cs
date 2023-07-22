using System;
using System.Collections.Generic;

namespace Conexion;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellidos { get; set; }

    public int? Edad { get; set; }

    public string Email { get; set; } = null!;

    public byte[] Contraseña { get; set; } = null!;
}
