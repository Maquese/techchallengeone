using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplication.Services;
using Aplication.Models;
using Domain.Exceptions;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public ClienteController()
        {
            
        }
        
        [HttpGet]
        public async Task<IActionResult> BuscarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]string cpf)
        {            
            try
            {
                var cliente = clienteAppService.VerificaCadastroCliente(cpf);
                return Ok(cliente?.Nome);
            }
            catch (DomainException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Erro de Validação",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]ClienteModel cliente)
        {
            try
            {
                var id = await clienteAppService.CriarCliente(cliente);
                return Ok(new { id = id });
            }
            catch (DomainException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Erro de Validação",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]VeiculoModel veiculo)
        {
            await clienteAppService.AdicionarVeiculo(veiculo);
            return Ok("Veículo adicionado com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]string placa)
        {
            var veiculo = await clienteAppService.BuscarVeiculo(placa);
            return Ok(veiculo);
        }
    }
}
