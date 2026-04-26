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
            return Task.FromResult<IActionResult>(Ok("Ordem de serviço criada com sucesso!"));
        }

        [HttpPost]
        public Task<IActionResult> AdicionarPecaServicos([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] OrdemServicoModel ordemServicoModel)
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }

        [HttpPost]
        public Task<IActionResult> AdicionarServico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromBody] ServicoModel servicoModel)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AdicionarServico(servicoModel)));
        }

        [HttpPost]
        public Task<IActionResult> AtribuirMecanicoEmDiagnostico([FromServices]OrdemServicoAppServiceImp ordemServicoAppService, [FromQuery] int ordemServicoId, [FromQuery] string mecanicoAtribuido)
        {
            return Task.FromResult<IActionResult>(Ok(ordemServicoAppService.AtribuirMecanico(ordemServicoId, mecanicoAtribuido))); 
        }

        
    }
}
