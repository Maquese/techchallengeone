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
        public async Task<IActionResult> CriarOrdemServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] AddOrdemServicoModel ordemServicoModel)
        {
            var id = await ordemServicoAppService.AdicionarOrdemServico(ordemServicoModel);
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
        

        


        [HttpPost]
        public async Task<IActionResult> AdicionarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] ServicoModel servicoModel)
        {
            var id = await ordemServicoAppService.AdicionarServico(servicoModel);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromQuery]int id)
        {
            var servico = await ordemServicoAppService.BuscarServico(id);
            return Ok(servico);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] UpdateServicoModel servicoModel)
        {
            await ordemServicoAppService.AtualizarServico(servicoModel);
            return Ok("Serviço atualizado com sucesso");    
        }

        [HttpDelete]
        public async Task<IActionResult> InativarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromQuery]int id)
        {
            await ordemServicoAppService.InativarServico(id);
            return Ok("Serviço inativado com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> ListarServicosAtivos([FromServices]OrdemServicoAppServiceImp ordemServicoAppService)
        {
            var servicos = await ordemServicoAppService.ListarServicosAtivos();
            return Ok(servicos);
        }
    }
}
