using Aplication.Models;
using Aplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemEstoqueController : ControllerBase
    {
        public ItemEstoqueController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService, [FromBody] AddItemEstoqueModel itemEstoqueModel)
        {
            var id = await itemEstoqueAppService.AdicionarItemEstoque(itemEstoqueModel);
            return Ok($"Item de estoque adicionado com sucesso. ID: {id}");
        }
    }
}
