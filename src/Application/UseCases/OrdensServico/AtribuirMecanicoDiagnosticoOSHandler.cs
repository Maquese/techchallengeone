using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Exceptions;

namespace Application.UseCases.OrdensServico;

public class AtribuirMecanicoDiagnosticoOSHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public AtribuirMecanicoDiagnosticoOSHandler(OrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<BaseResponse> Handle(AtribuiMecanicoRequest atribuiEmDiagnostico)
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

        return new BaseResponse
        {
            Success = true,
            Message = $"Mecânico '{atribuiEmDiagnostico.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmDiagnostico.OrdemServicoId}.",
            Data = atribuiEmDiagnostico.OrdemServicoId
        };
    } 
}


