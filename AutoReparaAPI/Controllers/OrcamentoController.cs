using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplication.Services;
using Aplication.Models;

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
        public async Task<IActionResult> CriarOrcamento([FromServices] OrcamentoAppServiceImp appService, [FromBody] AddOrcamentoModel model)
        {
            var id = await appService.AddOrcamento(model);
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> AprovarOrcamento([FromServices] OrcamentoAppServiceImp appService, [FromBody] int orcamentoId)
        {
            await appService.AprovarOrcamento(orcamentoId);
            return Ok("Orçamento aprovado com sucesso");
        }

        [HttpPost]
        public async Task<IActionResult> PagamentoEfetuado([FromServices] OrcamentoAppServiceImp appService, [FromBody] int orcamentoId)
        {
            await appService.PagarOrcamento(orcamentoId);
            return Ok("Orçamento pago com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> ListarOrcamentos([FromServices] OrcamentoAppServiceImp appService)
        {
            var orcamentos = await appService.ListarOrcamentos();
            return Ok(orcamentos);
        }
    }
}
