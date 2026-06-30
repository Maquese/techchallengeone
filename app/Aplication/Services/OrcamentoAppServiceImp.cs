using Aplication.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.InfraInterfaces;

namespace Aplication.Services;

public class OrcamentoAppServiceImp
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public OrcamentoAppServiceImp(OrcamentoRepository orcamentoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<int> AddOrcamento(AddOrcamentoModel model)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(model.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {model.OrdemServicoId} não encontrada.");
        }
        var orcamento = new Orcamento
        (model.OrdemServicoId, 100,"obs");
        await _orcamentoRepository.Adicionar(orcamento);
        return orcamento.Id;
    }

    public async Task AprovarOrcamento(int orcamentoId)
    {
        var orcamento = await _orcamentoRepository.ObterPorId(orcamentoId);
        if (orcamento == null)
        {
            throw new DomainException($"Orçamento com ID {orcamentoId} não encontrado.");
        }

        if(orcamento.DataDecisaoClientePagamento != null)
        {
            throw new DomainException($"Orçamento com ID {orcamentoId} já foi decidido pelo cliente.");
        }

        if(orcamento.DataDecisaoClienteAprovacao != null)
        {
            throw new DomainException($"Orçamento com ID {orcamentoId} aprovação já decidida.");
        }

        var ordemServico = await _ordemServicoRepository.ObterPorId(orcamento.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {orcamento.OrdemServicoId} não encontrada.");
        }

        if(ordemServico.Status != "Aguardando aprovação")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Aguardando aprovação' para aprovação de orçamento.");
        }

        orcamento.AprovarOrcamento();
        ordemServico.OSAprovada();
        await _orcamentoRepository.Atualizar(orcamento);
    }

    public async Task PagarOrcamento(int orcamentoId)
    {
        var orcamento = await _orcamentoRepository.ObterPorId(orcamentoId);
        if (orcamento == null)
        {
            throw new DomainException($"Orçamento com ID {orcamentoId} não encontrado.");
        }

        if(orcamento.DataDecisaoClientePagamento != null)
        {
            throw new DomainException($"Orçamento com ID {orcamentoId} já foi decidido pelo cliente.");
        }

        var ordemServico = await _ordemServicoRepository.ObterPorId(orcamento.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {orcamento.OrdemServicoId} não encontrada.");
        }

        if(ordemServico.Status != "Finalizada")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Orçamento Aprovado' para pagamento de orçamento.");
        }

        orcamento.MarcarOrcamentoPago();
        ordemServico.OrdemServicoEntregue();
        await _orcamentoRepository.Atualizar(orcamento);
    }

    public async Task<List<ListOrcamentoModel>> ListarOrcamentos()
    {
        var orcamentos = await _orcamentoRepository.ListarAtivos();
        return orcamentos.Select(o => new ListOrcamentoModel
        {
            Id = o.Id,
            OrdemServicoId = o.OrdemServicoId,
            Valor = o.ValorTotal,
            OrcamentoAprovado = o.OrcamentoAprovado,
            OrcamentoPago = o.OrcamentoPago,
            DataCadastro = o.DataCadastro
        }).ToList();
    }

}
