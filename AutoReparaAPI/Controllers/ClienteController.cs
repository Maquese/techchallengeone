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
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]AddClienteModel cliente)
        {
            var id = await clienteAppService.CriarCliente(cliente);
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]UpdateClienteModel cliente)
        {
            await clienteAppService.AtualizarCliente(cliente);
            return Ok("Cliente atualizado com sucesso");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarCliente([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]int id)
        {
            await clienteAppService.InativarCliente(id);
            return Ok("Cliente inativado com sucesso");
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

        [HttpPost]
        public async Task<IActionResult> AtualizarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromBody]UpdateVeiculoModel veiculo)
        {
            await clienteAppService.AtualizarVeiculo(veiculo);
            return Ok("Veículo atualizado com sucesso");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarVeiculo([FromServices]ClienteAppServiceImp clienteAppService, [FromQuery]int id)
        {
            await clienteAppService.InativarVeiculo(id);
            return Ok("Veículo inativado com sucesso");
        }
    }
}
