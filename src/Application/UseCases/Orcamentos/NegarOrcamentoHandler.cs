using Aplication.Interfaces;
using Application.Models.Responses;

namespace Application.UseCases.Orcamentos;

public class NegarOrcamentoHandler
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public NegarOrcamentoHandler(OrcamentoRepository orcamentoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<BaseResponse> Handle(int orcamentoId)
    {
        var orcamento = await _orcamentoRepository.ObterPorId(orcamentoId);
        if (orcamento == null)
        {
            throw new Exception($"Orçamento com ID {orcamentoId} não encontrado.");
        }

        if(orcamento.DataDecisaoClientePagamento != null)
        {
            throw new Exception($"Orçamento com ID {orcamentoId} já foi decidido pelo cliente.");
        }

        if(orcamento.DataDecisaoClienteAprovacao != null)
        {
            throw new Exception($"Orçamento com ID {orcamentoId} já decidida.");
        }

        var ordemServico = await _ordemServicoRepository.ObterPorId(orcamento.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new Exception($"Ordem de serviço com ID {orcamento.OrdemServicoId} não encontrada.");
        }

        orcamento.NegarOrcamento();
        ordemServico.OSNegada();
        try
        {
            await _ordemServicoRepository.Atualizar(ordemServico);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar a ordem de serviço com ID {ordemServico.Id}: {ex.Message}");
        }
        await _orcamentoRepository.Atualizar(orcamento);
        return new BaseResponse
        {
            Success = true,
            Message = "Orçamento negado com sucesso."
        };
    }
}
