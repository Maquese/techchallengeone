using System;
using Xunit;
using Domain.Entidades;

namespace Domain.Tests
{
    public class ServicoTests
    {
        [Fact]
        public void Construtor_DeveInicializarCorretamente()
        {
            // Arrange
            string descricao = "Troca de óleo";
            decimal valor = 150.50m;
            int tempoEstimado = 30;

            // Act
            var servico = new Servico(descricao, valor, tempoEstimado);

            // Assert
            Assert.Equal(descricao, servico.Descricao);
            Assert.Equal(valor, servico.Valor);
            Assert.Equal(tempoEstimado, servico.TempoEstimado);
            Assert.True(servico.Ativo);
        }

        [Fact]
        public void Atualizar_DeveAlterarValoresCorretamente()
        {
            // Arrange
            var servico = new Servico("Troca de óleo", 150.50m, 30);

            // Act
            servico.Atualizar("Revisão completa", 500m, 120);

            // Assert
            Assert.Equal("Revisão completa", servico.Descricao);
            Assert.Equal(500m, servico.Valor);
            Assert.Equal(120, servico.TempoEstimado);
        }

        [Fact]
        public void Construtor_DeveDefinirAtivoComoTrue()
        {
            // Act
            var servico = new Servico("Balanceamento", 80m, 20);

            // Assert
            Assert.True(servico.Ativo);
        }
    }
}
