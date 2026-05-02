using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            // Use formato Mercosul válido
            var veiculo = new Veiculo(new PlacaVO("ABC1D23"), "Modelo", "Marca", 2020, 1);

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
            var servico = new Servico("Troca de óleo", 100, 60);

            _servicoRepoMock.Setup(r => r.Adicionar(It.IsAny<Servico>())).Returns(Task.CompletedTask);

            var result = await _service.AdicionarServico(servicoModel);

            Assert.Equal(servicoModel.Descricao, result.Descricao);
            Assert.Equal(servicoModel.Valor, result.Valor);
            Assert.Equal(servicoModel.TempoEstimado, result.TempoEstimado);
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

        // -----------------------------
        // Testes dos métodos solicitados
        // -----------------------------

        [Fact]
        public async Task ListarServicosAtivos_DeveRetornarListaDeServicoModel()
        {
            var servicos = new List<Servico>
            {
                new Servico("Troca de óleo", 100, 30),
                new Servico("Revisão", 200, 60)
            };

            _servicoRepoMock.Setup(r => r.ListarAtivos()).ReturnsAsync(servicos);

            var result = await _service.ListarServicosAtivos();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Troca de óleo", result[0].Descricao);
            Assert.Equal(100, result[0].Valor);
            Assert.Equal(30, result[0].TempoEstimado);
        }

        [Fact]
        public async Task StatusAtualOrdensServico_ClienteNaoEncontrado_DeveLancarExcecao()
        {
            _clienteRepoMock.Setup(r => r.ObterPorId(10)).ReturnsAsync((Cliente?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.StatusAtualOrdensServico(10));
        }

        [Fact]
        public async Task StatusAtualOrdensServico_ClienteEncontrado_DeveRetornarListaStatus()
        {
            var cliente = new Cliente(
                new DocumentoVO("11144477735"),
                "João",
                new EmailVO("teste@dominio.com"),
                new CelularVO("11999999999")
            );

            var veiculo = new Veiculo(new PlacaVO("ABC1D23"), "Civic", "Honda", 2020, 1);
            SetPrivateProperty(cliente, "Veiculos", new List<Veiculo> { veiculo });

            var ordem = CreateOrdemServicoWithVeiculo(veiculo, "Em andamento", DateTime.UtcNow);

            _clienteRepoMock.Setup(r => r.ObterPorId(cliente.Id)).ReturnsAsync(cliente);
            _ordemRepoMock.Setup(r => r.ListarOrdensServicoPorCliente(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<OrdemServico> { ordem });

            var result = await _service.StatusAtualOrdensServico(cliente.Id);

            Assert.NotNull(result);
            Assert.Single(result);
            var status = result.First();
            Assert.Equal(ordem.Id, status.Id);
            Assert.Equal("Em andamento", status.Status);
            Assert.Equal(ordem.DataAbertura, status.DataCriacao);
            Assert.Equal(veiculo.Placa.Valor, status.PlacaVeiculo);
        }

        [Fact]
        public async Task TempoMedioExecucao_SemOrdensFinalizadas_DeveRetornarZero()
        {
            _ordemRepoMock.Setup(r => r.ListarOrdensServicoPorStatus(It.IsAny<List<string>>()))
                .ReturnsAsync(new List<OrdemServico>());

            var result = await _service.TempoMedioExecucao();

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task TempoMedioExecucao_ComOrdensFinalizadas_DeveRetornarMedia()
        {
            var now = DateTime.UtcNow;

            var ordem1 = new OrdemServico(1);
            SetPrivateProperty(ordem1, "DataInicioExecucao", now.AddMinutes(-60));
            SetPrivateProperty(ordem1, "DataFimExecucao", now);
            SetPrivateProperty(ordem1, "Status", "Finalizada");

            var ordem2 = new OrdemServico(2);
            SetPrivateProperty(ordem2, "DataInicioExecucao", now.AddMinutes(-30));
            SetPrivateProperty(ordem2, "DataFimExecucao", now);
            SetPrivateProperty(ordem2, "Status", "Entregue");

            var ordens = new List<OrdemServico> { ordem1, ordem2 };

            _ordemRepoMock.Setup(r => r.ListarOrdensServicoPorStatus(It.IsAny<List<string>>()))
                .ReturnsAsync(ordens);

            var result = await _service.TempoMedioExecucao();

            Assert.Equal(45, result);
        }

        // Helpers
        private static void SetPrivateProperty<T>(object target, string propertyName, T value)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null)
                throw new InvalidOperationException($"Property '{propertyName}' not found on type {target.GetType().FullName}.");
            prop.SetValue(target, value);
        }

        private static OrdemServico CreateOrdemServicoWithVeiculo(Veiculo veiculo, string status, DateTime dataAbertura)
        {
            var ordem = new OrdemServico(veiculo.Id);
            SetPrivateProperty(ordem, "Veiculo", veiculo);
            SetPrivateProperty(ordem, "Status", status);
            SetPrivateProperty(ordem, "DataAbertura", dataAbertura);

            var idProp = ordem.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (idProp != null && idProp.CanWrite)
            {
                idProp.SetValue(ordem, 1);
            }
            else
            {
                var field = ordem.GetType().GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null) field.SetValue(ordem, 1);
            }

            return ordem;
        }

        [Fact]
        public async Task DiagnosticoFinalizado_DeveAtualizarOrdemEAdicionarOrcamento()
        {
            // Arrange
            var ordem = new OrdemServico(1);
            SetPrivateProperty(ordem, "Servicos", new List<Servico> { new Servico("Troca de óleo", 100, 30) });

            var diagnosticoModel = new DiagnosticoFinalizadoModel
            {
                Id = 1,
                ItensEstoque = new List<AddItensOrdemServicoModel>
                {
                    new AddItensOrdemServicoModel { id = 10, quantidade = 2 }
                }
            };

            // Cria item de estoque usando o construtor correto
            var itemEstoque = new ItemEstoque(
                tipo: "Peça",
                nome: "Filtro de óleo",
                descricao: "Filtro para motor",
                valor: 50,
                unidadeMedida: "unidade"
            );
            // Força Id via reflection
            SetPrivateProperty(itemEstoque, "Id", 10);

            _ordemRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(ordem);
            _servicoRepoMock.Setup(r => r.ListarPorIds(It.IsAny<List<int>>()))
                .ReturnsAsync(ordem.Servicos.ToList());
            _itemRepoMock.Setup(r => r.ListarPorIds(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<ItemEstoque> { itemEstoque });
            _ordemRepoMock.Setup(r => r.Atualizar(ordem)).Returns(Task.CompletedTask);
            _orcamentoRepoMock.Setup(r => r.Adicionar(It.IsAny<Orcamento>())).Returns(Task.CompletedTask);

            // Act
            await _service.DiagnosticoFinalizado(diagnosticoModel);

            // Assert
            _ordemRepoMock.Verify(r => r.Atualizar(ordem), Times.Once);
            _orcamentoRepoMock.Verify(r => r.Adicionar(It.IsAny<Orcamento>()), Times.Once);

            // Confirma que a ordem foi diagnosticada
            Assert.Equal("Aguardando Aprovação", ordem.Status);
            Assert.NotNull(ordem.OrdemServicoItensEstoque);
            Assert.Single(ordem.OrdemServicoItensEstoque);

            // Confirma que o orçamento foi calculado corretamente
            var valorEsperado = 100 /* serviço */ + (50 * 2) /* item estoque */;
            _orcamentoRepoMock.Verify(r => r.Adicionar(It.Is<Orcamento>(o => o.ValorTotal == valorEsperado)), Times.Once);
        }
        [Fact]
        public async Task DeduzirItensEstoque_ItemNaoEncontrado_DeveLancarDomainException()
        {
            var itens = new List<AddItensOrdemServicoModel>
            {
                new AddItensOrdemServicoModel { id = 99, quantidade = 1 }
            };

            _itemRepoMock.Setup(r => r.ObterPorId(99)).ReturnsAsync((ItemEstoque?)null);

            await Assert.ThrowsAsync<DomainException>(() =>
                InvokePrivateDeduzirItensEstoque(itens));
        }

        [Fact]
        public async Task DeduzirItensEstoque_ItemEncontrado_DeveDeduzirEAtualizar()
        {
            var itens = new List<AddItensOrdemServicoModel>
            {
                new AddItensOrdemServicoModel { id = 10, quantidade = 2 }
            };

            var itemEstoque = new ItemEstoque(
                tipo: "Peça",
                nome: "Filtro de óleo",
                descricao: "Filtro para motor",
                valor: 50,
                unidadeMedida: "unidade"
            );
            SetPrivateProperty(itemEstoque, "Id", 10);
            SetPrivateProperty(itemEstoque, "QuantidadeEmEstoque", 5);

            _itemRepoMock.Setup(r => r.ObterPorId(10)).ReturnsAsync(itemEstoque);
            _itemRepoMock.Setup(r => r.Atualizar(itemEstoque)).Returns(Task.CompletedTask);

            await InvokePrivateDeduzirItensEstoque(itens);

            // Confirma que a quantidade foi deduzida
            Assert.Equal(3, itemEstoque.QuantidadeEmEstoque);

            // Confirma que o repositório foi chamado
            _itemRepoMock.Verify(r => r.Atualizar(itemEstoque), Times.Once);
        }
        private async Task InvokePrivateDeduzirItensEstoque(List<AddItensOrdemServicoModel> itens)
        {
            var method = typeof(OrdemServicoAppServiceImp)
                .GetMethod("DeduzirItensEstoque", BindingFlags.NonPublic | BindingFlags.Instance);

            var task = (Task)method.Invoke(_service, new object[] { itens });
            await task;
        }

        [Fact]
        public async Task AtualizarServico_ServicoNaoEncontrado_DeveLancarDomainException()
        {
            var model = new UpdateServicoModel
            {
                Id = 99,
                Descricao = "Troca de óleo",
                Valor = 120,
                TempoEstimado = 45
            };

            _servicoRepoMock.Setup(r => r.ObterPorId(99)).ReturnsAsync((Servico?)null);

            await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarServico(model));
        }

        [Fact]
        public async Task AtualizarServico_ServicoEncontrado_DeveAtualizarEChamarRepositorio()
        {
            var servico = new Servico("Troca de óleo", 100, 30);
            SetPrivateProperty(servico, "Id", 1);

            var model = new UpdateServicoModel
            {
                Id = 1,
                Descricao = "Troca de óleo premium",
                Valor = 150,
                TempoEstimado = 40
            };

            _servicoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(servico);
            _servicoRepoMock.Setup(r => r.Atualizar(servico)).Returns(Task.CompletedTask);

            await _service.AtualizarServico(model);

            // Confirma que o objeto foi atualizado
            Assert.Equal("Troca de óleo premium", servico.Descricao);
            Assert.Equal(150, servico.Valor);
            Assert.Equal(40, servico.TempoEstimado);

            // Confirma que o repositório foi chamado
            _servicoRepoMock.Verify(r => r.Atualizar(servico), Times.Once);
        }

    }
}
