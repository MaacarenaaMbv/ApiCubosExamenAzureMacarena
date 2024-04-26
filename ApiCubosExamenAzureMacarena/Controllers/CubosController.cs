using ApiCubosExamenAzureMacarena.Models;
using ApiCubosExamenAzureMacarena.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCubosExamenAzureMacarena.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {

        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpGet("{marca}")]
        public async Task<ActionResult<List<Cubo>>> FindCubo(string marca)
        {
            return await this.repo.FindCuboAsync(marca);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UsuarioCubo>> PerfilUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(x => x.Type == "UserData");
            string jsonUsuario = claim.Value;

            UsuarioCubo usuario = JsonConvert.DeserializeObject<UsuarioCubo>(jsonUsuario);
            return usuario;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CompraCubos>>> PedidosUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(x => x.Type == "UserData");
            string jsonUsuario = claim.Value;

            UsuarioCubo usuario = JsonConvert.DeserializeObject<UsuarioCubo>(jsonUsuario);
            return await this.repo.PedidosUsuario(usuario);
        }

    }
}
