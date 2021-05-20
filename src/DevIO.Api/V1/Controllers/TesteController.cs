using DevIO.Api.Controllers;
using DevIO.Api.Model;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    public class TesteController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Valor()
        {
            var user = new User { Nome = "Fernando" };
            return Ok(user);
        }
    }
}