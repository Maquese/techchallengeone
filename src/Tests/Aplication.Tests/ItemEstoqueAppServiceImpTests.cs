using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Domain.Aggregates;
using Domain.Exceptions;
using Aplication.Interfaces;
using Aplication.UseCases.ItensEstoque;
using Application.Models.Requests;
using Application.UseCases.ItensEstoque;
using Application.Models.Responses;

namespace Aplication.Tests
{
    public class ItemEstoqueAppServiceImpTests
    {
        private readonly Mock<ItemEstoqueRepository> _repoMock;
        private readonly AdicionarItemEstoqueHandler adicionarItemEstoqueHandler;
        private readonly AdicionarQtdEstoqueItemEstoqueHandler adicionarQtdEstoqueItemEstoqueHandler;
        private readonly AtualizarItemEstoqueHandler atualizarItemEstoqueHandler;
        private readonly InativarItemEstoqueHandler inativarItemEstoqueHandler;
        private readonly ObterItemEstoqueHandler obterItemEstoqueHandler;
        private readonly ListarItemEstoqueHandler listarItemEstoqueHandler;

        public ItemEstoqueAppServiceImpTests()
        {
            _repoMock = new Mock<ItemEstoqueRepository>();
            adicionarItemEstoqueHandler = new AdicionarItemEstoqueHandler(_repoMock.Object);
            adicionarQtdEstoqueItemEstoqueHandler = new AdicionarQtdEstoqueItemEstoqueHandler(_repoMock.Object);
            atualizarItemEstoqueHandler = new AtualizarItemEstoqueHandler(_repoMock.Object);
            inativarItemEstoqueHandler = new InativarItemEstoqueHandler(_repoMock.Object);
            obterItemEstoqueHandler = new ObterItemEstoqueHandler(_repoMock.Object);
            listarItemEstoqueHandler = new ListarItemEstoqueHandler(_repoMock.Object);
        }

        [Fact]
        public async Task AdicionarItemEstoque_DeveAdicionarItemERetornarId()
        {
            var model = new AddItemEstoqueRequest
            {
                Tipo = "Peça",
                Nome = "Filtro de óleo",
                Descricao = "Filtro para motor",
                Valor = 50m,
                UnidadeMedida = "Unidade"
            };

            _repoMock.Setup(r => r.Adicionar(It.IsAny<ItemEstoque>()))
                .Returns(Task.CompletedTask);

            var id = await adicionarItemEstoqueHandler.Handle(model);

            _repoMock.Verify(r => r.Adicionar(It.IsAny<ItemEstoque>()), Times.Once);
        }

        [Fact]
        public async Task ListarItensEstoque_DeveRetornarItens()
        {
            var itens = new List<ItemEstoque>
            {
                new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade")
            };

            _repoMock.Setup(r => r.ListarAtivos()).ReturnsAsync(itens);

            var result = await listarItemEstoqueHandler.Handle();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ObterItemEstoque_DeveRetornarItem()
        {
            var item = new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade");

            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(item);

            var result = await obterItemEstoqueHandler.Handle(1);

            Assert.Equal(item.Nome,  ((ItemEstoqueResponse)result.Data).Nome);
        }

        [Fact]
        public async Task AtualizarItemEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            var model = new UpdateItemEstoqueRequest { Id = 1 };
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => atualizarItemEstoqueHandler.Handle(model));
        }

        [Fact]
        public async Task InativarItemEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => inativarItemEstoqueHandler.Handle(1));
        }

        [Fact]
        public async Task AdicionarQuantidadeEstoque_ItemNaoEncontrado_DeveLancarExcecao()
        {
            var model = new AddQuantidadeItemEstoqueRequest { Id = 1, Quantidade = 5 };
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync((ItemEstoque)null);

            await Assert.ThrowsAsync<DomainException>(() => adicionarQtdEstoqueItemEstoqueHandler.Handle(model));
        }

        [Fact]
        public async Task AdicionarQuantidadeEstoque_DeveAtualizarQuantidade()
        {
            var item = new ItemEstoque("Peça", "Filtro", "Filtro motor", 50m, "Unidade");
            _repoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(item);
            _repoMock.Setup(r => r.Atualizar(item)).Returns(Task.CompletedTask);

            var model = new AddQuantidadeItemEstoqueRequest { Id = 1, Quantidade = 10 };

            await adicionarQtdEstoqueItemEstoqueHandler.Handle(model);

            Assert.Equal(10, item.QuantidadeEmEstoque);
            _repoMock.Verify(r => r.Atualizar(item), Times.Once);
        }
    }
}
