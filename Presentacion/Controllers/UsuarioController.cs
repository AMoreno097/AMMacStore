using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsuarioController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Modelo.Usuario usuario, string password)
        {
            // Crear una instancia del algoritmo de hash bcrypt
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            if (usuario.Nombre != null)
            {
                // Insertar usuario en la base de datos
                usuario.Contraseña = passwordHash;
                Modelo.Result result = Negocio.Usuario.RegistrarUsuario(usuario);
                return View();
            }
            else
            {
                // Proceso de login
                Modelo.Result result = Negocio.Usuario.Login(usuario.Email);
                usuario = (Modelo.Usuario)result.Objeto;

                if (usuario.Contraseña.SequenceEqual(passwordHash))
                {
                    return RedirectToAction("Index", "Home", usuario);
                }
            }
            return View(usuario);
        }
        [HttpGet]
        public ActionResult MostrarUsuarios()
        {
            Modelo.Usuario resultUsuarios = new Modelo.Usuario();
            resultUsuarios.Usuarios = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5145/api/");

                var responseTask = client.GetAsync("Usuario/MostrarUsuario");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Modelo.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objetos)
                    {
                        Modelo.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelo.Usuario>(resultItem.ToString());
                        resultUsuarios.Usuarios.Add(resultItemList);

                    }
                }
            }
            return View(resultUsuarios);
        }
        [HttpGet]
        public ActionResult Formulario(int? IdUsuario)
        {
            Modelo.Usuario usuario = new Modelo.Usuario();

            if (IdUsuario == null)
            {

                return View(usuario);
            }
            else
            {
                Modelo.Result result = new Modelo.Result();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5145/api/");
                    var responseTask = client.GetAsync("Usuario/MostrarUsuarioId/" + IdUsuario);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<Modelo.Result>();
                        readTask.Wait();
                        Modelo.Usuario resultItemList = new Modelo.Usuario();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelo.Usuario>(readTask.Result.Objeto.ToString());
                        result.Objeto = resultItemList;


                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.MensajeError = "No existen registros en la tabla Usuarios";
                    }

                }
                if (result.Correct)
                {
                    usuario = (Modelo.Usuario)result.Objeto;

                    ViewBag.Message = "Ocurrio un error al hacer la consulta:";


                    return View(usuario);
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al hacer la consulta:" + result.MensajeError;
                    return View("Modal");
                }

            }


        }
        [HttpPost]
        public ActionResult Formulario(Modelo.Usuario usuario, string password)
        {
           
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
           
            var passwordHash = bcrypt.GetBytes(20);
            Modelo.Result result = new Modelo.Result();

            if (usuario.IdUsuario == 0)
            {
                // Insertar usuario en la base de datos
                usuario.Contraseña = passwordHash;
                Modelo.Result resultado = Negocio.Usuario.RegistrarUsuario(usuario);
                if (resultado.Correct)
                {
                    return RedirectToAction("MostrarUsuarios", "Usuario");
                }
            }
            else
            {
                usuario.Contraseña = passwordHash;
                Modelo.Result resultad = Negocio.Usuario.ModificarUsuario(usuario);
                

                if (resultad.Correct)
                {
                    return RedirectToAction("MostrarUsuarios","Usuario");
                }
                else
                {
                    
                }
            }
            return View(usuario);
        }
        [HttpGet]
        public ActionResult NewPassword(string email)
        {
            Modelo.Usuario usuario = new Modelo.Usuario();
            usuario.Email = email;
            return View(usuario);
        }
        [HttpPost]
        public ActionResult NewPassword(string password, string email)
        {
            var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            Modelo.Usuario usuario = new Modelo.Usuario();
            usuario.Email = email;
            usuario.Contraseña = passwordHash;

            Modelo.Result result = Negocio.Usuario.NewContraseña(usuario);

            return RedirectToAction("Login", "Usuario");
        }
        public ActionResult CambiarPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CambiarPassword(string email)
        {


            //validar que exista el email en la bd

            string emailOrigen = "aemorenoh97@gmail.com";

            MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
            mailMessage.IsBodyHtml = true;

            string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));
            mailMessage.Body = contenidoHTML;
            string url = "http://localhost:5058/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
            //string url = "http://192.168.0.157/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "pmqwlnrlhwkvdpnw");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            ViewBag.Modal = "show";
            ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            return RedirectToAction("Login", "Usuario");



        }
        [HttpGet]
        public ActionResult Eliminar(int IdUsuario)
        {
            Modelo.Result result = Negocio.Usuario.EliminarUsuario(IdUsuario);


            if (result.Correct)
            {

                ViewBag.Message = "El usuario se elimino correctamente";
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al momento de eliminar al usuario" + result.MensajeError;

            }
            return RedirectToAction("MostrarUsuarios", "Usuario");

        }
        public ActionResult Cerrar()
        {
            return RedirectToAction("Login", "Usuario");

        }
    }
}
