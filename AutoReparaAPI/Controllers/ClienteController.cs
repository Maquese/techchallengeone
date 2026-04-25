using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplication.Services;
using Aplication.Models;

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
        public Task<IActionResult> BuscarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]string cpf)
        {            
            return Task.FromResult<IActionResult>(Ok("Hello World" + clienteAppService.VerificaCadastroCliente(cpf)?.Nome));
        }

        [HttpPost]
        public Task<IActionResult> CriarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]ClienteModel cliente)
        {
            clienteAppService.CriarCliente(cliente);
            return Task.FromResult<IActionResult>(Ok("Cliente criado com sucesso"));
        }

        [HttpPost]
        public Task<IActionResult> AdicionarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]VeiculoModel veiculo)
        {
            clienteAppService.AdicionarVeiculo(veiculo);
            return Task.FromResult<IActionResult>(Ok("Veículo adicionado com sucesso"));
        }

        [HttpGet]
        public Task<IActionResult> BuscarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]string placa)
        {
            var veiculo = clienteAppService.BuscarVeiculo(placa);
            return Task.FromResult<IActionResult>(Ok(veiculo));
        }
    }
}
