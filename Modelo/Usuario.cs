using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Usuario
    {
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "*Este campo es obligatoria*")]
        [RegularExpression("^[a-zA-z]+$", ErrorMessage = "*Este campo solo acepta letras*")]
        public string Nombre { get; set; }

        [RegularExpression("^[a-zA-z]+$", ErrorMessage = "*Este campo solo acepta letras*")]
        public string Apellidos { get; set; }

        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Edad { get; set; }
        [Required(ErrorMessage = "*Este campo es obligatoria*")]

        [EmailAddress]
        public string Email { get; set; }
        public byte[] Contraseña { get; set; }

        public List<object> Usuarios { get; set; }
    }
}