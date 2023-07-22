using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Negocio
{
    public class Usuario
    {
        public static Modelo.Result RegistrarUsuario(Modelo.Usuario usuario)
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using(Conexion.AmorenoMacStoreContext Context = new Conexion.AmorenoMacStoreContext())
                {
                    int query = Context.Database.ExecuteSqlRaw($"RegistrarUsuario '{usuario.Nombre}','{usuario.Apellidos}', {usuario.Edad},'{usuario.Email}', @Password", new SqlParameter("@Password", usuario.Contraseña));
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch(Exception Ex)
            {

            }

            return result;
        }
        public static Modelo.Result Login(string Email)
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using(Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"ConsultarUsuarioEmail '{Email}'").AsEnumerable().FirstOrDefault();
                    
                    if(query != null)
                    {
                        Modelo.Usuario usuario = new Modelo.Usuario();

                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.Apellidos = query.Apellidos;
                        usuario.Edad = query.Edad.Value;
                        usuario.Email = query.Email;
                        usuario.Contraseña = query.Contraseña;

                        result.Objeto = usuario;

                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch(Exception Ex)
            {

            }
            return result;
        }
        public static Modelo.Result MostrarUsuario()
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using(Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"ConsultarUsuarios");
                    if(query != null)
                    {
                        result.Objetos = new List<object>();
                        foreach(var item in query)
                        {
                            Modelo.Usuario usuario = new Modelo.Usuario();
                            usuario.IdUsuario = item.IdUsuario;
                            usuario.Nombre = item.Nombre;
                            usuario.Apellidos = item.Apellidos;
                            usuario.Edad = item.Edad.Value;
                            usuario.Email = item.Email;
                            usuario.Contraseña = item.Contraseña;

                            result.Objetos.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {

                    }
                }
            }
            catch(Exception Ex)
            {

            }

            return result;

        }
        public static Modelo.Result MostrarUsuarioId(int IdUsuario)
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using (Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"ConsultarUsuarioId '{IdUsuario}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        Modelo.Usuario usuario = new Modelo.Usuario();

                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.Apellidos = query.Apellidos;
                        usuario.Edad = query.Edad.Value;
                        usuario.Email = query.Email;
                        usuario.Contraseña = query.Contraseña;

                        result.Objeto = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception Ex)
            {

            }
            return result;
        }
        public static Modelo.Result EliminarUsuario(int IdUsuario)
        {

            Modelo.Result result = new Modelo.Result();
            try
            {
                using (Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"EliminarUsuario {IdUsuario}");

                    if (query != 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.MensajeError = "Ocurrió un error al eliminar al usuario";
                    }
                }
                return result;
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.MensajeError = ex.Message;

            }
            return new Modelo.Result();

        }

        public static Modelo.Result Update(Modelo.Usuario usuario)
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using (Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ModificarUsuario {usuario.IdUsuario}, '{usuario.Nombre}', '{usuario.Apellidos}', {usuario.Edad}, '{usuario.Edad}', @Password", new SqlParameter("@Password", usuario.Contraseña));

                    if (query >= 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.MensajeError = "Ocurrió un error al modificar al usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.MensajeError = ex.Message;

            }

            return result;
        }
        public static Modelo.Result NewContraseña(Modelo.Usuario usuario)
        {
            Modelo.Result result = new Modelo.Result();
            try
            {
                using (Conexion.AmorenoMacStoreContext context = new Conexion.AmorenoMacStoreContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"NewPasswordUpDate '{usuario.Email}', @Password", new SqlParameter("@Password", usuario.Contraseña));
                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch
            {

            }
            return result;
        }
    }
    
}