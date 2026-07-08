using System.Threading.Tasks;
using Xunit;
using Moq;
using Aplication.Services;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.Entidades;
using Aplication.Interfaces;
using Domain.VOs;
using Aplication.UseCases.Clientes;
using Application.Models.Requests;

namespace Aplication.Tests
{
    public class ClienteAppServiceImpTests
    {
        private readonly Mock<ClienteRepository> _clienteRepoMock;
        private readonly Mock<VeiculoRepository> _veiculoRepoMock;
        private readonly AdicionarClienteHandler _adicionarClienteHandler;

        private readonly AtualizarClienteHandler _atualizarClienteHandler;
        private readonly InativarClienteHandler _inativarClienteHandler;
        private readonly VerificaCadastroClienteHandler _verificaCadastroClienteHandler;
        private readonly AdicionarVeiculoClienteHandler _adicionarVeiculoHandler;
        private readonly AtualizarVeiculoClienteHandler _atualizarVeiculoHandler;
        private readonly InativarVeiculoClienteHandler _inativarVeiculoHandler;
        private readonly BuscarVeiculoPlacaClienteHandler _buscarVeiculoPlacaHandler;

        public ClienteAppServiceImpTests()
        {
            _clienteRepoMock = new Mock<ClienteRepository>();
            _veiculoRepoMock = new Mock<VeiculoRepository>();
            _adicionarClienteHandler = new AdicionarClienteHandler(_clienteRepoMock.Object);
            _atualizarClienteHandler = new AtualizarClienteHandler(_clienteRepoMock.Object);
            _inativarClienteHandler = new InativarClienteHandler(_clienteRepoMock.Object);
            _verificaCadastroClienteHandler = new VerificaCadastroClienteHandler(_clienteRepoMock.Object);
            _adicionarVeiculoHandler = new AdicionarVeiculoClienteHandler(_veiculoRepoMock.Object, _clienteRepoMock.Object);
            _atualizarVeiculoHandler = new AtualizarVeiculoClienteHandler(_veiculoRepoMock.Object);
            _inativarVeiculoHandler = new InativarVeiculoClienteHandler(_veiculoRepoMock.Object);
            _buscarVeiculoPlacaHandler = new BuscarVeiculoPlacaClienteHandler(_veiculoRepoMock.Object);
        }

        [Fact]
        public async Task CriarCliente_DeveAdicionarClienteERetornarId()
        {
            var model = new AddClienteRequest
            {
                Documento = "11144477735",
                Nome = "João",
                Email = "teste@dominio.com",
                Celular = "11999999999"
            };

            _clienteRepoMock.Setup(r => r.Adicionar(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);

            var id = await _adicionarClienteHandler.Handle(model);

            _clienteRepoMock.Verify(r => r.Adicionar(It.IsAny<Cliente>()), Times.Once);
            Assert.True(id >= 0);
        }

        [Fact]
        public async Task AtualizarCliente_ClienteNaoEncontrado_DeveLancarExcecao()
        {
            var model = new UpdateClienteModel { Id = 1 };
            _clienteRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<DomainException>(() => _atualizarClienteHandler.Handle(model));
        }

        [Fact]
        public async Task AtualizarCliente_ClienteEncontrado_DeveAtualizar()
        {
            var cliente = new Cliente(
                new DocumentoVO("11144477735"),
                "Antigo",
                new EmailVO("teste@dominio.com"),
                new CelularVO("11999999999")
            );

            _clienteRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(cliente);

            var model = new UpdateClienteModel 
            { 
                Id = 1, 
                Documento = "11144477735", // <-- ajuste aqui
                Nome = "Novo", 
                Email = "novo@dominio.com", 
                Celular = "11988888888" 
            };

            await _atualizarClienteHandler.Handle(model);

            _clienteRepoMock.Verify(r => r.Atualizar(It.IsAny<Cliente>()), Times.Once);
        }


        [Fact]
        public async Task InativarCliente_ClienteNaoEncontrado_DeveLancarExcecao()
        {
            _clienteRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<DomainException>(() => _inativarClienteHandler.Handle(1));
        }

        [Fact]
        public async Task VerificaCadastroCliente_ClienteNaoEncontrado_DeveRetornarNull()
        {
            _clienteRepoMock.Setup(r => r.ObterPorDocumento("11144477735"))
                .ReturnsAsync((Cliente)null);

            var result = await _verificaCadastroClienteHandler.Handle("11144477735");

            Assert.Null(result); // ajuste: agora verificamos null
        }

        [Fact]
        public async Task VerificaCadastroCliente_ClienteEncontrado_DeveRetornarUpdateClienteModel()
        {
            var cliente = new Cliente(
                new DocumentoVO("11144477735"),
                "João",
                new EmailVO("teste@dominio.com"),
                new CelularVO("11999999999")
            );

            _clienteRepoMock.Setup(r => r.ObterPorDocumento("11144477735"))
                .ReturnsAsync(cliente);

            var result = await _verificaCadastroClienteHandler.Handle("11144477735");

            Assert.NotNull(result); // ajuste: verificamos objeto
            Assert.Equal(cliente.Id, result.Id);
            Assert.Equal(cliente.Documento.Numero, result.Documento);
            Assert.Equal(cliente.Nome, result.Nome);
            Assert.Equal(cliente.Email.Endereco, result.Email);
            Assert.Equal(cliente.Celular.Numero, result.Celular);
        }

        [Fact]
        public async Task AdicionarVeiculo_DeveAdicionarVeiculo()
        {
            var model = new AddVeiculoRequest
            {
                Placa = "ABC-1234",
                Modelo = "Civic",
                Marca = "Honda",
                Ano = 2020,
                ClienteId = 1
            };

            _veiculoRepoMock.Setup(r => r.Adicionar(It.IsAny<Veiculo>()))
                .Returns(Task.CompletedTask);

            await _adicionarVeiculoHandler.Handle(model);

            _veiculoRepoMock.Verify(r => r.Adicionar(It.IsAny<Veiculo>()), Times.Once);
        }

        [Fact]
        public async Task BuscarVeiculo_VeiculoNaoEncontrado_DeveRetornarNull()
        {
            _veiculoRepoMock.Setup(r => r.BuscarPorPlaca("ABC-1234"))
                .ReturnsAsync((Veiculo)null);

            var result = await _buscarVeiculoPlacaHandler.Handle("ABC-1234");

            Assert.Null(result);
        }

        [Fact]
        public async Task BuscarVeiculo_VeiculoEncontrado_DeveRetornarObjeto()
        {
            var veiculo = new Veiculo(
                new PlacaVO("ABC-1234"),
                "Civic",
                "Honda",
                2020,
                1
            );

            _veiculoRepoMock.Setup(r => r.BuscarPorPlaca("ABC-1234")).ReturnsAsync(veiculo);

            var result = await _buscarVeiculoPlacaHandler.Handle("ABC-1234");

            Assert.NotNull(result);
            Assert.Equal("ABC-1234", result.Placa);
        }

        [Fact]
        public async Task InativarVeiculo_VeiculoNaoEncontrado_DeveLancarExcecao()
        {
            _veiculoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<DomainException>(() => _inativarVeiculoHandler.Handle(1));
        }

        [Fact]
        public async Task AtualizarVeiculo_VeiculoEncontrado_DeveAtualizar()
        {
            var veiculo = new Veiculo(
                new PlacaVO("XYZ-9876"),
                "Antigo",
                "Ford",
                2015,
                1
            );

            _veiculoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(veiculo);

            var model = new UpdateVeiculoModel { Id = 1, Modelo = "Novo", Marca = "Ford", Ano = 2022, Placa = "XYZ-9876" };
            await _atualizarVeiculoHandler.Handle(model);

            _veiculoRepoMock.Verify(r => r.Atualizar(It.IsAny<Veiculo>()), Times.Once);
        }
    }
}
