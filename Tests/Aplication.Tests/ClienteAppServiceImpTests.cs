using System.Threading.Tasks;
using Xunit;
using Moq;
using Aplication.Services;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.VOs;
using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Aplication.Tests
{
    public class ClienteAppServiceImpTests
    {
        private readonly Mock<ClienteRepository> _clienteRepoMock;
        private readonly Mock<VeiculoRepository> _veiculoRepoMock;
        private readonly ClienteAppServiceImp _service;

        public ClienteAppServiceImpTests()
        {
            _clienteRepoMock = new Mock<ClienteRepository>();
            _veiculoRepoMock = new Mock<VeiculoRepository>();
            _service = new ClienteAppServiceImp(_clienteRepoMock.Object, _veiculoRepoMock.Object);
        }

        [Fact]
        public async Task CriarCliente_DeveAdicionarClienteERetornarId()
        {
            // Arrange
            var model = new AddClienteModel
            {
                Documento = "11144477735", // CPF válido
                Nome = "João",
                Email = "teste@dominio.com",
                Celular = "11999999999"
            };

            _clienteRepoMock.Setup(r => r.Adicionar(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);

            // Act
            var id = await _service.CriarCliente(model);

            // Assert
            _clienteRepoMock.Verify(r => r.Adicionar(It.IsAny<Cliente>()), Times.Once);
            Assert.True(id >= 0); // Id é gerado pelo domínio/persistência
        }

        [Fact]
        public async Task AtualizarCliente_ClienteNaoEncontrado_DeveLancarExcecao()
        {
            var model = new UpdateClienteModel { Id = 1 };
            _clienteRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarCliente(model));
        }

        [Fact]
        public async Task InativarCliente_ClienteNaoEncontrado_DeveLancarExcecao()
        {
            _clienteRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.InativarCliente(1));
        }

        [Fact]
        public void VerificaCadastroCliente_ClienteNaoEncontrado_DeveRetornarNull()
        {
            _clienteRepoMock.Setup(r => r.ObterPorDocumento("11144477735"))
                .Returns((Cliente)null);

            var result = _service.VerificaCadastroCliente("11144477735");

            Assert.Null(result);
        }

        [Fact]
        public async Task AdicionarVeiculo_DeveAdicionarVeiculo()
        {
            var model = new VeiculoModel
            {
                Placa = "ABC-1234",
                Modelo = "Civic",
                Marca = "Honda",
                Ano = 2020,
                ClienteId = 1
            };

            _veiculoRepoMock.Setup(r => r.Adicionar(It.IsAny<Veiculo>()))
                .Returns(Task.CompletedTask);

            await _service.AdicionarVeiculo(model);

            _veiculoRepoMock.Verify(r => r.Adicionar(It.IsAny<Veiculo>()), Times.Once);
        }

        [Fact]
        public async Task BuscarVeiculo_VeiculoNaoEncontrado_DeveRetornarNull()
        {
            _veiculoRepoMock.Setup(r => r.BuscarPorPlaca("ABC-1234"))
                .ReturnsAsync((Veiculo)null);

            var result = await _service.BuscarVeiculo("ABC-1234");

            Assert.Null(result);
        }

        [Fact]
        public async Task InativarVeiculo_VeiculoNaoEncontrado_DeveLancarExcecao()
        {
            _veiculoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.InativarVeiculo(1));
        }
    }
}
