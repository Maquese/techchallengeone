using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Aplication.Services;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.InfraInterfaces;

namespace Aplication.Tests
{
    public class ItemEstoqueAppServiceImpTests
    {
        private readonly Mock<ItemEstoqueRepository> _repoMock;
        private readonly ItemEstoqueAppServiceImp _service;

        public ItemEstoqueAppServiceImpTests()
        {
            _repoMock = new Mock<ItemEstoqueRepository>();
            _service = new ItemEstoqueAppServiceImp(_repoMock.Object);
        }

        [Fact]
        public async Task AdicionarItemEstoque_DeveAdicionarItemERetornarId()
        {
            var model = new AddItemEstoqueModel
            {
                Tipo = "Peça",
                Nome = "Filtro de óleo",
                Descricao = "Filtro para motor",
                Valor = 50m,
                UnidadeMedida = "Unidade"
            };

            _repoMock.Setup(r => r.Adicionar(It.IsAny<ItemEstoque>()))
                .Returns(Task.CompletedTask);

            var id = await _service.AdicionarItemEstoque(model);

            _repoMock.Verify(r => r.Adicionar(It.IsAny<ItemEstoque>()), Times.Once);
            Assert.True(id >= 0);
        }

        [Fact]
        public async Task ListarItensEstoque_DeveRetornarItens()
        {
            var itens = new List<ItemEstoque>
            {
                new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade")
            };

            _repoMock.Setup(r => r.ListarAtivos()).ReturnsAsync(itens);

            var result = await _service.ListarItensEstoque();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ObterItemEstoque_DeveRetornarItem()
        {
            var item = new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade");

            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(item);

            var result = await _service.ObterItemEstoque(1);

            Assert.Equal(item, result);
        }

        [Fact]
        public async Task AtualizarItemEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            var model = new UpdateItemEstoqueModel { Id = 1 };
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarItemEstoque(model));
        }

        [Fact]
        public async Task InativarItemEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.InativarItemEstoque(1));
        }

        [Fact]
        public async Task AdicionarQuantidadeEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            var model = new AddQuantidadeItemEstoqueModel { Id = 1, Quantidade = 5 };
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AdicionarQuantidadeEstoque(model));
        }

        [Fact]
        public async Task AdicionarQuantidadeEstoque_DeveAtualizarQuantidade()
        {
            var item = new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade");
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(item);
            _repoMock.Setup(r => r.Atualizar(item)).Returns(Task.CompletedTask);

            var model = new AddQuantidadeItemEstoqueModel { Id = 1, Quantidade = 10 };

            await _service.AdicionarQuantidadeEstoque(model);

            Assert.Equal(10, item.QuantidadeEmEstoque);
            _repoMock.Verify(r => r.Atualizar(item), Times.Once);
        }
    }
}
