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
        public async Task<IActionResult> AdicionarVeiculo([FromServices]AdicionarVeiculoClienteHandler adicionarVeiculoClienteHandler, [FromBody]VeiculoModel veiculo)
        {
            await adicionarVeiculoClienteHandler.Handle(veiculo);
            return Ok("Veículo adicionado com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarVeiculo([FromServices]VerificaCadastroClienteHandler verificaCadastroVeiculoHandler, [FromQuery]string placa)
        {
            var veiculo = await verificaCadastroVeiculoHandler.Handle(placa);
            return Ok(veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarVeiculo([FromServices]AtualizarVeiculoClienteHandler atualizarVeiculoClienteHandler, [FromBody]UpdateVeiculoModel veiculo)
        {
            await atualizarVeiculoClienteHandler.Handle(veiculo);
            return Ok("Veículo atualizado com sucesso");
        }

        [HttpDelete]
        public async Task<IActionResult> InativarVeiculo([FromServices]InativarVeiculoClienteHandler inativarVeiculoClienteHandler, [FromQuery]int id)
        {
            await inativarVeiculoClienteHandler.Handle(id);
            return Ok("Veículo inativado com sucesso");
        }
    }
}
