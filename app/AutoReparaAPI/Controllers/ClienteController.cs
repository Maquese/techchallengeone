using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplication.Services;
using Aplication.Models;
using Aplication.UseCases.Clientes;
using Aplication;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        public ClienteController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromServices]AdicionarClienteHandler adicionarClienteHandler, [FromBody]AddClienteModel cliente)
        {
            var id = await adicionarClienteHandler.Handle(cliente);
            return Ok(id);
        }
        
        [HttpPost]
        public async Task<IActionResult> AtualizarCliente([FromServices]AtualizarClienteHandler clienteAppService, [FromBody]UpdateClienteModel cliente)
        {
            await clienteAppService.Handle(cliente);
            return Ok("Cliente atualizado com sucesso");
        }
        
        [HttpGet]
        public async Task<IActionResult> BuscarCliente([FromServices]VerificaCadastroClienteHandler verificaCadastroClienteHandler, [FromQuery]string cpf)
        {            
            var cliente = await verificaCadastroClienteHandler.Handle(cpf);
            return Ok(cliente);
        }

        [HttpDelete]
        public async Task<IActionResult> InativarCliente([FromServices]InativarClienteHandler inativarClienteHandler, [FromQuery]int id)
        {
            await inativarClienteHandler.Handle(id);
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
