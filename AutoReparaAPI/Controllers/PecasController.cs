using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PecasController : ControllerBase
    {
        public PecasController()
        {
            
        }

        [HttpPut(Name = "AdicionarPeca")]
        public Task<IActionResult> AdicionarPeca()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }
    }
}
