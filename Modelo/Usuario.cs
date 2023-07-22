namespace Modelo
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Edad { get; set; }
        public string Email { get; set; }
        public byte[] Contraseña { get; set; }

        public List<object> Usuarios { get; set; }
    }
}