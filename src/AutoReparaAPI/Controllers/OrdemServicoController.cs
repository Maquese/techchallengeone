using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplication.Models;
using Aplication.Services;
using Aplication.UseCases.OrdensServico;
using Application.Models.Requests;
using Application.UseCases.OrdensServico;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrdemServicoController : ControllerBase
    {
        public OrdemServicoController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CriarOrdemServico([FromServices]AdicionarOrdemServicoHandler adicionarOrdemServicoHandler, [FromBody] AddOrdemServicoModel ordemServicoModel)
        {
            var id = await adicionarOrdemServicoHandler.Handle(ordemServicoModel);
            return Ok(id);
        }       

        [HttpPost]
        public async Task<IActionResult> AtribuirMecanicoEmDiagnostico([FromServices]AtribuirMecanicoDiagnosticoOSHandler atribuirMecanicoEmDiagnosticoHandler, [FromBody] AtribuiMecanicoModel atribuiEmDiagnostico)
        {
            await atribuirMecanicoEmDiagnosticoHandler.Handle(atribuiEmDiagnostico);
            return Ok("Mecânico atribuído ao diagnóstico");
        }

        [HttpPost]
        public async Task<IActionResult> DiagnosticoFInalizado([FromServices]FinalizarDiagnosticoOSHandler diagnosticoFinalizadoHandler, [FromBody] DiagnosticoFinalizadoModel diagnosticoFinalizadoModel)
        {
            await diagnosticoFinalizadoHandler.Handle(diagnosticoFinalizadoModel);
            return Ok("Diagnóstico finalizado com sucesso");
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirMecanicoEmExecucao([FromServices]AtribuirMecanicoExecucaoOSHandler atribuirMecanicoExecucaoOSHandler, [FromBody] AtribuiMecanicoModel atribuiEmExecucao)
        {
            await atribuirMecanicoExecucaoOSHandler.Handle(atribuiEmExecucao);
            return Ok("Mecânico atribuído à execução");
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarOrdemServico([FromServices]FinalizarOrdemServicoHandler finalizarOrdemServicoHandler, [FromBody] int ordemServicoId)
        {
            await finalizarOrdemServicoHandler.Handle(ordemServicoId);
            return Ok("Ordem de serviço finalizada com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> StatusAtualOrdensServicoCliente([FromServices]StatusAtualOSClienteHandler statusAtualOrdensServicoClienteHandler, [FromQuery] int clienteId)
        {
            var status = await statusAtualOrdensServicoClienteHandler.Handle(clienteId);
            return Ok(status);
        }
        
        [HttpGet]
        public async Task<IActionResult> TempoMedioExecucaoMinutos([FromServices]TempoMedioExecucaoOSHandler tempoMedioExecucaoOSHandler)
        {
            var tempoMedio = await tempoMedioExecucaoOSHandler.Handle();
            return Ok(tempoMedio);
        }

        [HttpGet]
        public async Task<IActionResult> ListarOrdensServicoPorStatus([FromServices]ListarOrdensServicoOrdenadoHandler listarOrdensServicoPorStatusHandler)
        {
            var ordensServico = await listarOrdensServicoPorStatusHandler.Handle();
            return Ok(ordensServico);
        }

        


        [HttpPost]
        public async Task<IActionResult> AdicionarServico([FromServices]AdicionarServicoHandler adicionarServicoOSHandler, [FromBody] ServicoModel servicoModel)
        {
            var id = await adicionarServicoOSHandler.Handle(servicoModel);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarServico([FromServices]BuscarServicoHandler buscarServicoHandler, [FromQuery]int id)
        {
            var servico = await buscarServicoHandler.Handle(id);
            return Ok(servico);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarServico([FromServices]AtualizarServicoHandler atualizarServicoHandler, [FromBody] UpdateServicoModel servicoModel)
        {
            await atualizarServicoHandler.Handle(servicoModel);
            return Ok("Serviço atualizado com sucesso");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarServico([FromServices]InativarServicoHandler inativarServicoHandler, [FromQuery]int id)
        {
            await inativarServicoHandler.Handle(id);
            return Ok("Serviço inativado com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> ListarServicosAtivos([FromServices]ListarServicosAtivosHandler listarServicosAtivosHandler)
        {
            var servicos = await listarServicosAtivosHandler.Handle();
            return Ok(servicos);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaStatusOrdemServico([FromServices]ConsutaStatusOSHandler consultaStatusOSHandler, [FromQuery]int ordemServicoId)
        {
            var status = await consultaStatusOSHandler.Handle(ordemServicoId);
            return Ok(status);
        }
    }
}
