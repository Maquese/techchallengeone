using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemEstoqueController : ControllerBase
    {
        public ItemEstoqueController()
        {
            
        }

        [HttpPut(Name = "AdicionarItemEstoque")]
        public Task<IActionResult> AdicionarItemEstoque()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }
    }
}
