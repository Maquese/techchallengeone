using System.Security.Cryptography.X509Certificates;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Aplication.Services;

public class OrdemServicoAppServiceImp
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ServicoRepository _servicoRepository;
    private readonly VeiculoRepository _veiculoRepository;
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly ItemEstoqueRepository _itensEstoqueRepository;
    private readonly ClienteRepository _clienteRepository;
    public OrdemServicoAppServiceImp(OrdemServicoRepository ordemServicoRepository, ServicoRepository servicoRepository,
                                     VeiculoRepository veiculoRepository, OrcamentoRepository orcamentoRepository,
                                     ItemEstoqueRepository itensEstoqueRepository, ClienteRepository clienteRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _servicoRepository = servicoRepository;
        _veiculoRepository = veiculoRepository;
        _orcamentoRepository = orcamentoRepository;
        _itensEstoqueRepository = itensEstoqueRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<int> AdicionarOrdemServico(AddOrdemServicoModel ordemServico)
    {
        var veiculo = await _veiculoRepository.ObterPorId(ordemServico.VeiculoId);
        if (veiculo == null)        {
            throw new DomainException($"Veículo com ID {ordemServico.VeiculoId} não encontrado.");
        }

        var ordemServicoEntity = new OrdemServico(
            ordemServico.VeiculoId,
            servicos: ordemServico.ServicosIds != null 
                ? await _servicoRepository.ListarPorIds(ordemServico.ServicosIds) 
                : new List<Servico>()
        );
        await _ordemServicoRepository.Adicionar(ordemServicoEntity); 
        return ordemServicoEntity.Id;
    }


    public async Task<string> AtribuirMecanico(AtribuiMecanicoModel atribuiEmDiagnostico)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(atribuiEmDiagnostico.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {atribuiEmDiagnostico.OrdemServicoId} não encontrada.");
        }

        if(ordemServico.Status != "Recebida")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Recebida' para atribuição de mecânico ao diagnóstico.");
        }

        ordemServico.OSEmDiagnostico(atribuiEmDiagnostico.MecanicoAtribuido);
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Mecânico '{atribuiEmDiagnostico.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmDiagnostico.OrdemServicoId}.";
    }

    public async Task<string> AtribuirMecanicoExecucao(AtribuiMecanicoModel atribuiEmReparo)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(atribuiEmReparo.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {atribuiEmReparo.OrdemServicoId} não encontrada.");
        }

        if (ordemServico.Status != "Aprovada")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Aprovada' para atribuição de mecânico à execução.");
        }

        ordemServico.EmExecucao(atribuiEmReparo.MecanicoAtribuido);
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Mecânico '{atribuiEmReparo.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmReparo.OrdemServicoId}.";
    }

    public async Task<string> FinalizarOrdemServico(int ordemServicoId)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(ordemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServicoId} não encontrada.");
        }

        if (ordemServico.Status != "Em execução")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Em execução' para finalização.");
        }

        await DeduzirItensEstoque(ordemServico.OrdemServicoItensEstoque?.Select(i => new AddItensOrdemServicoModel { id = i.ItemEstoqueId, quantidade = i.Quantidade }).ToList() ?? new List<AddItensOrdemServicoModel>());

        ordemServico.FinalizarOrdemServico();
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Ordem de serviço ID {ordemServicoId} finalizada com sucesso.";
    }

    private async Task DeduzirItensEstoque(List<AddItensOrdemServicoModel> itensEstoque)
    {   
        foreach (var item in itensEstoque)
        {
            var itemEstoque = await _itensEstoqueRepository.ObterPorId(item.id);
            if (itemEstoque == null)
            {
                throw new DomainException($"Item de estoque com ID {item.id} não encontrado.");
            }

            itemEstoque.DeduzirQuantidadeEstoque(item.quantidade);
            await _itensEstoqueRepository.Atualizar(itemEstoque);
        }
    }

      public async Task DiagnosticoFinalizado(DiagnosticoFinalizadoModel diagnosticoFinalizadoModel)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(diagnosticoFinalizadoModel.Id);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço para o veículo ID {diagnosticoFinalizadoModel.Id} não encontrada.");
        }

        if(ordemServico.Status != "Em diagnóstico")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Em diagnóstico' para finalização do diagnóstico.");
        }

        var itensEstoque = diagnosticoFinalizadoModel.ItensEstoque.Select(item => new OrdemServicoItemEstoque
        (diagnosticoFinalizadoModel.Id, item.id, item.quantidade)).ToList();
       
        ordemServico.OSDiagnosticada(itensEstoque);
        var orcamento = new Orcamento(
            diagnosticoFinalizadoModel.Id, 
            await CalcularValorTotalOrcamento(diagnosticoFinalizadoModel.ItensEstoque, ordemServico.Servicos?.Select(x => x.Id).ToList()),
            "obs");
        await _ordemServicoRepository.Atualizar(ordemServico);
        await _orcamentoRepository.Adicionar(orcamento);
    }

    private async Task<decimal> CalcularValorTotalOrcamento(List<AddItensOrdemServicoModel> itensEstoque, List<int> servicosIds)
    {
        var servicos = await _servicoRepository.ListarPorIds(servicosIds);

        var itens = await _itensEstoqueRepository.ListarPorIds(itensEstoque.Select(i => i.id).ToList());

        return servicos.Sum(s => s.Valor) + itens.Sum(i => i.Valor * itensEstoque.First(e => e.id == i.Id).quantidade);
    }



    #region Métodos relacionados a serviços dentro da ordem de serviço
    public async Task<ServicoModel> AdicionarServico(ServicoModel servico)
    {
        var servicoEntity = new Servico
        (
            descricao: servico.Descricao,
            valor: servico.Valor,
            tempoEstimado: servico.TempoEstimado
        );
        
        await _servicoRepository.Adicionar(servicoEntity);
        return servico;
    }
    public async Task<ServicoModel> BuscarServico(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            return null;
        }

        return new ServicoModel
        {
            Id = servico.Id,
            Descricao = servico.Descricao,
            Valor = servico.Valor,
            TempoEstimado = servico.TempoEstimado
        };
    }

    public async Task AtualizarServico(UpdateServicoModel servicoModel)
    {
        var servico = await _servicoRepository.ObterPorId(servicoModel.Id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        servico.Atualizar(servicoModel.Descricao, servicoModel.Valor, servicoModel.TempoEstimado);

        await _servicoRepository.Atualizar(servico);
    }

    public async Task InativarServico(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        await _servicoRepository.Inativar(servico);
    }

    public async Task<List<ServicoModel>> ListarServicosAtivos()
    {
        var servicos = await _servicoRepository.ListarAtivos();
        return servicos.Select(s => new ServicoModel
        {
            Id = s.Id,
            Descricao = s.Descricao,
            Valor = s.Valor,
            TempoEstimado = s.TempoEstimado
        }).ToList();
    }

    public async Task<IList<StatusOSsClienteModel>> StatusAtualOrdensServico(int clienteId)
    {
        var cliente = await _clienteRepository.ObterPorId(clienteId);
        if (cliente == null)        {
            throw new DomainException($"Cliente com ID {clienteId} não encontrado.");
        }
        var ordensServico = await _ordemServicoRepository.ListarOrdensServicoPorCliente(cliente.Veiculos.Select(v => v.Id).ToList());
        return ordensServico.Select(os => new StatusOSsClienteModel
        {
            Id = os.Id,
            Status = os.Status,
            DataCriacao = os.DataAbertura,
            PlacaVeiculo = os.Veiculo.Placa.Valor
        }).ToList();
    }

    public async Task<int> TempoMedioExecucao()
    {
        var ordensFinalizadas = await _ordemServicoRepository.ListarOrdensServicoPorStatus(new List<string> { "Finalizada", "Entregue" });
        if (!ordensFinalizadas.Any())
        {
            return 0;
        }

        var tempoTotalExecucao = ordensFinalizadas.Sum(os => (os.DataFimExecucao.Value - os.DataInicioExecucao.Value).TotalMinutes);
        return (int)(tempoTotalExecucao / ordensFinalizadas.Count());
    }

    #endregion


}
