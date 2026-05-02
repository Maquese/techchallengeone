using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Aplication.Services;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Domain.InfraInterfaces;
using Domain.VOs;

namespace Aplication.Tests
{
    public class OrdemServicoAppServiceImpTests
    {
        private readonly Mock<OrdemServicoRepository> _ordemRepoMock;
        private readonly Mock<ServicoRepository> _servicoRepoMock;
        private readonly Mock<VeiculoRepository> _veiculoRepoMock;
        private readonly Mock<OrcamentoRepository> _orcamentoRepoMock;
        private readonly Mock<ItemEstoqueRepository> _itemRepoMock;
        private readonly Mock<ClienteRepository> _clienteRepoMock;
        private readonly OrdemServicoAppServiceImp _service;

        public OrdemServicoAppServiceImpTests()
        {
            _ordemRepoMock = new Mock<OrdemServicoRepository>();
            _servicoRepoMock = new Mock<ServicoRepository>();
            _veiculoRepoMock = new Mock<VeiculoRepository>();
            _orcamentoRepoMock = new Mock<OrcamentoRepository>();
            _itemRepoMock = new Mock<ItemEstoqueRepository>();
            _clienteRepoMock = new Mock<ClienteRepository>();

            _service = new OrdemServicoAppServiceImp(
                _ordemRepoMock.Object,
                _servicoRepoMock.Object,
                _veiculoRepoMock.Object,
                _orcamentoRepoMock.Object,
                _itemRepoMock.Object,
                _clienteRepoMock.Object
            );
        }

        [Fact]
        public async Task AdicionarOrdemServico_VeiculoNaoEncontrado_DeveLancarExcecao()
        {
            var model = new AddOrdemServicoModel { VeiculoId = 1 };
            _veiculoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Veiculo?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AdicionarOrdemServico(model));
        }

        [Fact]
        public async Task AdicionarOrdemServico_DeveAdicionarERetornarId()
        {
            var model = new AddOrdemServicoModel { VeiculoId = 1 };
            var veiculo = new Veiculo(new PlacaVO("ABC-1234"), "Modelo", "Marca", 2020, 1); // ✅ placa válida

            _veiculoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(veiculo);
            _servicoRepoMock.Setup(r => r.ListarPorIds(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Servico>());
            _ordemRepoMock.Setup(r => r.Adicionar(It.IsAny<OrdemServico>()))
                .Returns(Task.CompletedTask);

            var id = await _service.AdicionarOrdemServico(model);

            _ordemRepoMock.Verify(r => r.Adicionar(It.IsAny<OrdemServico>()), Times.Once);
            Assert.True(id >= 0);
        }

        [Fact]
        public async Task AtribuirMecanico_OrdemNaoEncontrada_DeveLancarExcecao()
        {
            var model = new AtribuiMecanicoModel { OrdemServicoId = 1, MecanicoAtribuido = "Carlos" };
            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AtribuirMecanico(model));
        }

        [Fact]
        public async Task AtribuirMecanico_DeveAtualizarOrdem()
        {
            var ordem = new OrdemServico(1);
            var model = new AtribuiMecanicoModel { OrdemServicoId = 1, MecanicoAtribuido = "Carlos" };

            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordem);
            _ordemRepoMock.Setup(r => r.Atualizar(ordem)).Returns(Task.CompletedTask);

            var result = await _service.AtribuirMecanico(model);

            Assert.Contains("Carlos", result);
            Assert.Equal("Em Diagnostico", ordem.Status);
        }

        [Fact]
        public async Task AtribuirMecanicoExecucao_StatusInvalido_DeveLancarExcecao()
        {
            var ordem = new OrdemServico(1); // Status inicial = "Recebida"
            var model = new AtribuiMecanicoModel { OrdemServicoId = 1, MecanicoAtribuido = "João" };

            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordem);

            await Assert.ThrowsAsync<DomainException>(() => _service.AtribuirMecanicoExecucao(model));
        }

        [Fact]
        public async Task FinalizarOrdemServico_StatusInvalido_DeveLancarExcecao()
        {
            var ordem = new OrdemServico(1); // Status inicial = "Recebida"

            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordem);

            await Assert.ThrowsAsync<DomainException>(() => _service.FinalizarOrdemServico(1));
        }

        [Fact]
        public async Task DiagnosticoFinalizado_OrdemNaoEncontrada_DeveLancarExcecao()
        {
            var model = new DiagnosticoFinalizadoModel { Id = 1, ItensEstoque = new List<AddItensOrdemServicoModel>() };
            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.DiagnosticoFinalizado(model));
        }

        [Fact]
        public async Task AdicionarServico_DeveAdicionarERetornarModel()
        {
            var servicoModel = new ServicoModel { Descricao = "Troca de óleo", Valor = 100, TempoEstimado = 60 };
            _servicoRepoMock.Setup(r => r.Adicionar(It.IsAny<Servico>())).Returns(Task.CompletedTask);

            var result = await _service.AdicionarServico(servicoModel);

            Assert.Equal(servicoModel.Descricao, result.Descricao);
        }

        [Fact]
        public async Task BuscarServico_ServicoNaoEncontrado_DeveRetornarNull()
        {
            _servicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Servico?)null);

            var result = await _service.BuscarServico(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task InativarServico_ServicoNaoEncontrado_DeveLancarExcecao()
        {
            _servicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Servico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.InativarServico(1));
        }
    }
}
