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
        public async Task<IActionResult> AdicionarItemEstoque()
        {
            return Ok("Hello World");
        }
    }
}
