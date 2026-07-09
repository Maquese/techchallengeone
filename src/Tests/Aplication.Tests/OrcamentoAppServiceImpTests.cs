using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Domain.Aggregates;
using Application.Interfaces;
using Domain.Entidades;
using Domain.Exceptions;
using System.Reflection;
using Application.UseCases.Orcamentos;
using Application.Models;
using Application.Models.Requests;

namespace Aplication.Tests
{
    public class OrcamentoAppServiceImpTests
    {
        private readonly Mock<OrcamentoRepository> _orcamentoRepoMock;
        private readonly Mock<OrdemServicoRepository> _ordemServicoRepoMock;
        private readonly AdicionarOrcamentoHandler _adicionarOrcamentoHandler;
        private readonly AprovarOrcamentoHandler _aprovarOrcamentoHandler;
        private readonly PagarOrcamentoHandler _pagarOrcamentoHandler;

        public OrcamentoAppServiceImpTests()
        {
            _orcamentoRepoMock = new Mock<OrcamentoRepository>();
            _ordemServicoRepoMock = new Mock<OrdemServicoRepository>();
            _adicionarOrcamentoHandler = new AdicionarOrcamentoHandler(_orcamentoRepoMock.Object, _ordemServicoRepoMock.Object);
            _aprovarOrcamentoHandler = new AprovarOrcamentoHandler(_orcamentoRepoMock.Object, _ordemServicoRepoMock.Object);
            _pagarOrcamentoHandler = new PagarOrcamentoHandler(_orcamentoRepoMock.Object, _ordemServicoRepoMock.Object);
        }

        [Fact]
        public async Task AddOrcamento_OrdemServicoNaoEncontrada_DeveLancarDomainException()
        {
            var model = new AddOrcamentoRequest { OrdemServicoId = 1 };
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _adicionarOrcamentoHandler.Handle(model));
        }

        [Fact]
        public async Task AddOrcamento_DeveAdicionarOrcamentoERetornarId()
        {
            var model = new AddOrcamentoRequest { OrdemServicoId = 1 };
            var ordemServico = new OrdemServico(1);

            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Adicionar(It.IsAny<Orcamento>()))
                .Returns(Task.CompletedTask);

            var id = await _adicionarOrcamentoHandler.Handle(model);

            _orcamentoRepoMock.Verify(r => r.Adicionar(It.IsAny<Orcamento>()), Times.Once);
            Assert.True(id >= 0);
        }

        [Fact]
        public async Task AprovarOrcamento_OrcamentoNaoEncontrado_DeveLancarDomainException()
        {
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Orcamento?)null);

            await Assert.ThrowsAsync<DomainException>(() => _aprovarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task AprovarOrcamento_OrdemServicoNaoEncontrada_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((OrdemServico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _aprovarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task AprovarOrcamento_StatusInvalido_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1); // Status inicial = "Recebida"

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);

            await Assert.ThrowsAsync<DomainException>(() => _aprovarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task AprovarOrcamento_DeveAtualizarOrcamentoEOrdemServico()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1);
            ordemServico.OSDiagnosticada(new List<OrdemServicoItemEstoque>()); // muda status para "Aguardando aprovação"

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Atualizar(orcamento)).Returns(Task.CompletedTask);

            await _aprovarOrcamentoHandler.Handle(1);

            Assert.True(orcamento.OrcamentoAprovado);
            Assert.Equal("Aprovada", ordemServico.Status);
            _orcamentoRepoMock.Verify(r => r.Atualizar(orcamento), Times.Once);
        }

        [Fact]
        public async Task PagarOrcamento_OrcamentoNaoEncontrado_DeveLancarDomainException()
        {
            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((Orcamento?)null);

            await Assert.ThrowsAsync<DomainException>(() => _pagarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task PagarOrcamento_DeveAtualizarOrcamentoEOrdemServico()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1);
            // Corrigido: status precisa ser "Finalizada" para permitir pagamento
            SetPrivateProperty(ordemServico, "Status", "Finalizada");

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);
            _orcamentoRepoMock.Setup(r => r.Atualizar(orcamento)).Returns(Task.CompletedTask);

            await _pagarOrcamentoHandler.Handle(1);

            Assert.True(orcamento.OrcamentoPago);
            Assert.Equal("Entregue", ordemServico.Status);
            _orcamentoRepoMock.Verify(r => r.Atualizar(orcamento), Times.Once);
        }

        // Helper para setar propriedades privadas
        private static void SetPrivateProperty<T>(object target, string propertyName, T value)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null)
                throw new InvalidOperationException($"Property '{propertyName}' not found on type {target.GetType().FullName}.");
            prop.SetValue(target, value);
        }

        [Fact]
        public async Task AprovarOrcamento_JaDecididoPagamento_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            // Força DataDecisaoClientePagamento para simular já decidido
            SetPrivateProperty(orcamento, "DataDecisaoClientePagamento", DateTime.UtcNow);

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);

            await Assert.ThrowsAsync<DomainException>(() => _aprovarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task AprovarOrcamento_JaDecididoAprovacao_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            // Força DataDecisaoClienteAprovacao para simular já decidido
            SetPrivateProperty(orcamento, "DataDecisaoClienteAprovacao", DateTime.UtcNow);

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);

            await Assert.ThrowsAsync<DomainException>(() => _aprovarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task PagarOrcamento_JaDecididoPagamento_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            // Força DataDecisaoClientePagamento para simular já pago
            SetPrivateProperty(orcamento, "DataDecisaoClientePagamento", DateTime.UtcNow);

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);

            await Assert.ThrowsAsync<DomainException>(() => _pagarOrcamentoHandler.Handle(1));
        }

        [Fact]
        public async Task PagarOrcamento_StatusInvalido_DeveLancarDomainException()
        {
            var orcamento = new Orcamento(1, 100, "obs");
            var ordemServico = new OrdemServico(1);
            // Status inicial = "Recebida", inválido para pagamento
            SetPrivateProperty(ordemServico, "Status", "Recebida");

            _orcamentoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(orcamento);
            _ordemServicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordemServico);

            await Assert.ThrowsAsync<DomainException>(() => _pagarOrcamentoHandler.Handle(1));
        }

    }
}
