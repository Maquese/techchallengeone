using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Aplication.Services;
using Aplication.Models;
using Domain.Aggregates;
using Domain.InfraInterfaces;
using Domain.Entidades;

namespace Aplication.Tests
{
    public class OrcamentoAppServiceImpTests
    {
        private readonly Mock<OrcamentoRepository> _orcamentoRepoMock;
        private readonly Mock<OrdemServicoRepository> _ordemServicoRepoMock;
        private readonly OrcamentoAppServiceImp _service;

        public OrcamentoAppServiceImpTests()
        {
            _orcamentoRepoMock = new Mock<OrcamentoRepository>();
            _ordemServicoRepoMock = new Mock<OrdemServicoRepository>();
            _service = new OrcamentoAppServiceImp(_orcamentoRepoMock.Object, _ordemServicoRepoMock.Object);
        }

        [Fact]
        public async Task AddOrcamento_OrdemServicoNaoEncontrada_DeveLancarExcecao()
        {
            var model = new AddOrcamentoModel { OrdemServicoId = 1 };
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.AddOrcamento(model));
        }

        [Fact]
        public async Task AddOrcamento_DeveAdicionarOrcamentoERetornarId()
        {
            var model = new AddOrcamentoModel { OrdemServicoId = 1 };
            var ordemServico = new OrdemServico(1);

            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Adicionar(It.IsAny<Orcamento>()))
                .Returns(Task.CompletedTask);

            var id = await _service.AddOrcamento(model);

            _orcamentoRepoMock.Verify(r => r.Adicionar(It.IsAny<Orcamento>()), Times.Once);
            Assert.True(id >= 0);
        }

        [Fact]
        public async Task AprovarOrcamento_OrcamentoNaoEncontrado_DeveLancarExcecao()
        {
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Orcamento?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.AprovarOrcamento(1));
        }

        [Fact]
        public async Task AprovarOrcamento_OrdemServicoNaoEncontrada_DeveLancarExcecao()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.AprovarOrcamento(1));
        }

        [Fact]
        public async Task AprovarOrcamento_StatusInvalido_DeveLancarExcecao()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1); // Status inicial = "Recebida"

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);

            await Assert.ThrowsAsync<Exception>(() => _service.AprovarOrcamento(1));
        }

        [Fact]
        public async Task AprovarOrcamento_DeveAtualizarOrcamentoEOrdemServico()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1);
            ordemServico.OSDiagnosticada(new List<OrdemServicoItemEstoque>()); // muda status para "Aguardando Aprovação"

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Atualizar(orcamento)).Returns(Task.CompletedTask);

            await _service.AprovarOrcamento(1);

            Assert.True(orcamento.OrcamentoAprovado);
            Assert.Equal("Aprovada", ordemServico.Status);
            _orcamentoRepoMock.Verify(r => r.Atualizar(orcamento), Times.Once);
        }

        [Fact]
        public async Task PagarOrcamento_OrcamentoNaoEncontrado_DeveLancarExcecao()
        {
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Orcamento?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.PagarOrcamento(1));
        }

        [Fact]
        public async Task PagarOrcamento_DeveAtualizarOrcamentoEOrdemServico()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1);

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Atualizar(orcamento)).Returns(Task.CompletedTask);

            await _service.PagarOrcamento(1);

            Assert.True(orcamento.OrcamentoPago);
            Assert.Equal("Entregue", ordemServico.Status);
            _orcamentoRepoMock.Verify(r => r.Atualizar(orcamento), Times.Once);
        }
    }
}
