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

        [HttpGet]
        public async Task<IActionResult> ListarItensEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService)
        {
            var itensEstoque = await itemEstoqueAppService.ListarItensEstoque();
            return Ok(itensEstoque);
        }

         [HttpGet]
        public async Task<IActionResult> ObterItemEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService,[FromQuery] int id)
        {
            var itemEstoque = await itemEstoqueAppService.ObterItemEstoque(id);
            return Ok(itemEstoque);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarItemEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService, [FromBody] UpdateItemEstoqueModel itemEstoqueModel)
        {
            await itemEstoqueAppService.AtualizarItemEstoque(itemEstoqueModel);
            return Ok("Item de estoque atualizado com sucesso.");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarItemEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService, [FromQuery] int id)
        {
            await itemEstoqueAppService.InativarItemEstoque(id);
            return Ok("Item de estoque inativado com sucesso.");
        }
    }
}
