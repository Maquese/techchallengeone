using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplication;
using Aplication.UseCases.ItensEstoque;
using Application.Models.Requests;
using Application.UseCases.ItensEstoque;

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
        public async Task<IActionResult> AdicionarItemEstoque([FromServices]AdicionarItemEstoqueHandler itemEstoqueAppService, [FromBody] AddItemEstoqueRequest itemEstoqueModel)
        {
            var id = await itemEstoqueAppService.Handle(itemEstoqueModel);
            return Ok($"Item de estoque adicionado com sucesso. ID: {id}");
        }

        [HttpGet]
        public async Task<IActionResult> ListarItensEstoque([FromServices]ListarItemEstoqueHandler itemEstoqueAppService)
        {
            var itensEstoque = await itemEstoqueAppService.Handle();
            return Ok(itensEstoque);
        }

         [HttpGet]
        public async Task<IActionResult> ObterItemEstoque([FromServices]ObterItemEstoqueHandler itemEstoqueAppService,[FromQuery] int id)
        {
            var itemEstoque = await itemEstoqueAppService.Handle(id);
            return Ok(itemEstoque);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarItemEstoque([FromServices]AtualizarItemEstoqueHandler itemEstoqueAppService, [FromBody] UpdateItemEstoqueRequest itemEstoqueModel)
        {
            await itemEstoqueAppService.Handle(itemEstoqueModel);
            return Ok("Item de estoque atualizado com sucesso.");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarItemEstoque([FromServices]InativarItemEstoqueHandler itemEstoqueAppService, [FromQuery] int id)
        {
            await itemEstoqueAppService.Handle(id);
            return Ok("Item de estoque inativado com sucesso.");
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarQuantidadeEstoque([FromServices]AdicionarQtdEstoqueItemEstoqueHandler itemEstoqueAppService, [FromBody] AddQuantidadeItemEstoqueRequest adicionarQuantidadeModel)
        {
            await itemEstoqueAppService.Handle(adicionarQuantidadeModel);
            return Ok("Quantidade adicionada ao estoque com sucesso.");
        }
    }
}
