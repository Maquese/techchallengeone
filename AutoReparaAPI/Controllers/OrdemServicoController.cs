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
        public async Task<IActionResult> CriarOrdemServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] OrdemServicoModel ordemServicoModel)
        {
            var id = await ordemServicoAppService.AdicionarOrdemServico(ordemServicoModel);
            return Ok(id);
        }

        // [HttpPost]
        // public async Task<IActionResult> AdicionarPecaServicos([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] OrdemServicoModel ordemServicoModel)
        // {
        //     var id = await ordemServicoAppService.AdicionarPecaServicos(ordemServicoModel);
        //     return Ok(id);
        // }

        [HttpPost]
        public async Task<IActionResult> AdicionarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] ServicoModel servicoModel)
        {
            var id = await ordemServicoAppService.AdicionarServico(servicoModel);
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirMecanicoEmDiagnostico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] AtribuiMecanicoModel atribuiEmDiagnostico)
        {
            await ordemServicoAppService.AtribuirMecanico(atribuiEmDiagnostico);
            return Ok("Mecânico atribuído ao diagnóstico");
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirMecanicoEmExecucao([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] AtribuiMecanicoModel atribuiEmExecucao)
        {
            await ordemServicoAppService.AtribuirMecanicoExecucao(atribuiEmExecucao);
            return Ok("Mecânico atribuído à execução");
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarOrdemServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] int ordemServicoId)
        {
            await ordemServicoAppService.FinalizarOrdemServico(ordemServicoId);
            return Ok("Ordem de serviço finalizada com sucesso");
        }
        
    }
}
