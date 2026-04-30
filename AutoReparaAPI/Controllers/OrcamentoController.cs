using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplication.Services;
using Aplication.Models;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
    }
}
