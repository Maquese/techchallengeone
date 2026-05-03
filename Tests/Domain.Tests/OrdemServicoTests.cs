using System;
using System.Collections.Generic;
using Xunit;
using Domain.Aggregates;
using Domain.Entidades;

namespace Domain.Tests
{
    public class OrdemServicoTests
    {
        [Fact]
        public void Construtor_DeveInicializarCorretamente()
        {
            // Arrange
            int veiculoId = 1;

            // Act
            var ordem = new OrdemServico(veiculoId);

            // Assert
            Assert.Equal(veiculoId, ordem.VeiculoId);
            Assert.Equal("Recebida", ordem.Status);
            Assert.NotNull(ordem.Servicos);
            Assert.NotNull(ordem.OrdemServicoItensEstoque);
            Assert.NotNull(ordem.Orcamentos);
            Assert.True(ordem.Ativo);
            Assert.True(ordem.DataAbertura <= DateTime.Now);
            Assert.Null(ordem.DataFechamento);
        }

        [Fact]
        public void OSEmDiagnostico_DeveDefinirStatusEDadosCorretos()
        {
            var ordem = new OrdemServico(1);

            ordem.OSEmDiagnostico("Carlos");

            Assert.Equal("Em diagnóstico", ordem.Status);
            Assert.Equal("Carlos", ordem.MecanicoAtribuido);
        }

        [Fact]
        public void OSDiagnosticada_DeveDefinirItensEStatusCorretos()
        {
            var ordem = new OrdemServico(1);
            var itens = new List<OrdemServicoItemEstoque>
            {
                new OrdemServicoItemEstoque(1, 1, 2)
            };

            ordem.OSDiagnosticada(itens);

            Assert.Equal("Aguardando aprovação", ordem.Status);
            Assert.Equal(itens, ordem.OrdemServicoItensEstoque);
            Assert.Null(ordem.MecanicoAtribuido);
        }

        [Fact]
        public void OSAprovada_DeveDefinirStatusComoAprovada()
        {
            var ordem = new OrdemServico(1);

            ordem.OSAprovada();

            Assert.Equal("Aprovada", ordem.Status);
        }

        [Fact]
        public void EmExecucao_DeveDefinirStatusEDadosCorretos()
        {
            var ordem = new OrdemServico(1);

            ordem.EmExecucao("João");

            Assert.Equal("Em execução", ordem.Status);
            Assert.Equal("João", ordem.MecanicoAtribuido);
            Assert.NotNull(ordem.DataInicioExecucao);
        }

        [Fact]
        public void FinalizarOrdemServico_DeveDefinirStatusEDadosCorretos()
        {
            var ordem = new OrdemServico(1);

            ordem.FinalizarOrdemServico();

            Assert.Equal("Finalizada", ordem.Status);
            Assert.NotNull(ordem.DataFimExecucao);
        }

        [Fact]
        public void OrdemServicoEntregue_DeveDefinirStatusEDadosCorretos()
        {
            var ordem = new OrdemServico(1);

            ordem.OrdemServicoEntregue();

            Assert.Equal("Entregue", ordem.Status);
            Assert.NotNull(ordem.DataFechamento);
        }
    }
}
