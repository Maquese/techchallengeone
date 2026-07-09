
using Domain.Exceptions;
using Application.Interfaces;

namespace Application.UseCases.Orcamentos;

public class PagarOrcamentoHandler
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public PagarOrcamentoHandler(OrcamentoRepository orcamentoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task Handle(int orcamentoId)
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
}
