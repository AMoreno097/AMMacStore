using Microsoft.AspNetCore.Mvc;

namespace Servicios.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        [Route("api/Usuario/MostrarUsuario")]
        public IActionResult GetAll()
        {
            Modelo.Usuario usuario = new Modelo.Usuario();
            Modelo.Result result = Negocio.Usuario.MostrarUsuario();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
        [HttpGet]
        [Route("api/Usuario/MostrarUsuarioId/{IdUsuario}")]
        public IActionResult GetById(int IdUsuario)
        {

            Modelo.Result result = Negocio.Usuario.MostrarUsuarioId(IdUsuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
            //return View();
        }
    }
}
