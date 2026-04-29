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
        public Task<IActionResult> CriarOrcamento([FromServices] OrcamentoAppServiceImp appService, [FromBody] AddOrcamentoModel model)
        {
            return Task.FromResult<IActionResult>(Ok(appService.AddOrcamento(model))) ;
        }

        [HttpPost]
        public Task<IActionResult> AprovarOrcamento([FromServices] OrcamentoAppServiceImp appService, [FromBody] int orcamentoId)
        {
            return Task.FromResult<IActionResult>(Ok(appService.AprovarOrcamento(orcamentoId))) ;
        }
    }
}
