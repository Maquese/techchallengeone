using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplication.Services;
using Aplication.UseCases.Orcamentos;
using Application.Models;
using Application.Models.Requests;
using Application.UseCases.Orcamentos;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrcamentoController : ControllerBase
    {
        public OrcamentoController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CriarOrcamento([FromServices] AdicionarOrcamentoHandler adicionarOrcamentoHandler, [FromBody] AddOrcamentoRequest model)
        {
            var id = await adicionarOrcamentoHandler.Handle(model);
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> AprovarOrcamento([FromServices] AprovarOrcamentoHandler aprovarOrcamentoHandler, [FromBody] int orcamentoId)
        {
            await aprovarOrcamentoHandler.Handle(orcamentoId);
            return Ok("Orçamento aprovado com sucesso");
        }

            [HttpPost]
        public async Task<IActionResult> NegarOrcamento([FromServices] NegarOrcamentoHandler negarOrcamentoHandler, [FromBody] int orcamentoId)
        {
            await negarOrcamentoHandler.Handle(orcamentoId);
            return Ok("Orçamento negado com sucesso");
        }

        [HttpPost]
        public async Task<IActionResult> PagamentoEfetuado([FromServices] PagarOrcamentoHandler pagarOrcamentoHandler, [FromBody] int orcamentoId)
        {
            await pagarOrcamentoHandler.Handle(orcamentoId);
            return Ok("Orçamento pago com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> ListarOrcamentos([FromServices] ListarOrcamentoHandler listarOrcamentoHandler)
        {
            var orcamentos = await listarOrcamentoHandler.Handle();
            return Ok(orcamentos);
        }
    }
}
