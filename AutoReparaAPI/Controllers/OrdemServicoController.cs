using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplication.Models;
using Aplication.Services;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdemServicoController : ControllerBase
    {
        public OrdemServicoController()
        {
            
        }

        [HttpPost]
        public Task<IActionResult> CriarOrdemServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] OrdemServicoModel ordemServicoModel)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AdicionarOrdemServico(ordemServicoModel)));
        }

        // [HttpPost]
        // public Task<IActionResult> AdicionarPecaServicos([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] OrdemServicoModel ordemServicoModel)
        // {
        //     return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AdicionarPecaServicos(ordemServicoModel)));
        // }

        [HttpPost]
        public Task<IActionResult> AdicionarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] ServicoModel servicoModel)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AdicionarServico(servicoModel)));
        }

        [HttpPost]
        public Task<IActionResult> AtribuirMecanicoEmDiagnostico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] AtribuiMecanicoModel atribuiEmDiagnostico)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AtribuirMecanico(atribuiEmDiagnostico))); 
        }

        [HttpPost]
        public Task<IActionResult> AtribuirMecanicoEmExecucao([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] AtribuiMecanicoModel atribuiEmExecucao)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AtribuirMecanicoExecucao(atribuiEmExecucao)));
        }

        [HttpPost]
        public Task<IActionResult> FinalizarOrdemServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] int ordemServicoId)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.FinalizarOrdemServico(ordemServicoId)));
        }
        
    }
}
