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
        public async Task<IActionResult> BuscarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]string cpf)
        {            
            var cliente = clienteAppService.VerificaCadastroCliente(cpf);
            return Ok(cliente?.Nome);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]ClienteModel cliente)
        {
            var id = await clienteAppService.CriarCliente(cliente);
            return Ok(id);
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
