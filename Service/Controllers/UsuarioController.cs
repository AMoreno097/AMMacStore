using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace Service.Controllers
{
    public class UsuarioController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Usuario/MostrarUsuario")]
        public IHttpActionResult MostrarUsuarios()
        {
            Modelo.Result result = Negocio.Usuario.MostrarUsuario();

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Usuario/MostrarUsuarioId/{IdUsuario}")]
        public IHttpActionResult MostrarUsuarioId(int IdUsuario)
        {
            Modelo.Result result = Negocio.Usuario.MostrarUsuarioId(IdUsuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
