using Aplication.Models;
using Aplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> AdicionarQuantidadeEstoque([FromServices]ItemEstoqueAppServiceImp itemEstoqueAppService, [FromBody] AddQuantidadeItemEstoqueModel adicionarQuantidadeModel)
        {
            await itemEstoqueAppService.AdicionarQuantidadeEstoque(adicionarQuantidadeModel);
            return Ok("Quantidade adicionada ao estoque com sucesso.");
        }
    }
}
