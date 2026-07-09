using Application.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;

namespace Application.UseCases.OrdensServico;

public class AtribuirMecanicoExecucaoOSHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public AtribuirMecanicoExecucaoOSHandler(OrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<string> Handle(AtribuiMecanicoRequest atribuiEmReparo)
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

}
