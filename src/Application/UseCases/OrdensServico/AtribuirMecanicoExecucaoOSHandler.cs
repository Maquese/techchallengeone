using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.UseCases.OrdensServico;

public class AtribuirMecanicoExecucaoOSHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ILogger<AtribuirMecanicoExecucaoOSHandler> _logger;
    public AtribuirMecanicoExecucaoOSHandler(OrdemServicoRepository ordemServicoRepository,
                                             ILogger<AtribuirMecanicoExecucaoOSHandler> logger)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse> Handle(AtribuiMecanicoRequest atribuiEmReparo)
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
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = Activity.Current?.TraceId.ToString() ?? "n/a",
            ["event_type"] = "order_processing",
            ["operation"] = "start_order_execution",
            ["order_id"] = ordemServico.Id,
            ["vehicle_id"] = ordemServico.VeiculoId,
            ["status"] = ordemServico.Status
        });
        ordemServico.EmExecucao(atribuiEmReparo.MecanicoAtribuido);
        await _ordemServicoRepository.Atualizar(ordemServico);
        _logger.LogInformation(
            "Mecânico '{Mecanico}' atribuído à ordem de serviço ID {OrderId}. VehicleId: {VehicleId}, Status: {Status}, Data: {Data}",
            atribuiEmReparo.MecanicoAtribuido,
            ordemServico.Id,
            ordemServico.VeiculoId,
            ordemServico.Status,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        return new BaseResponse
        {
            Success = true,
            Message = $"Mecânico '{atribuiEmReparo.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmReparo.OrdemServicoId}.",
            Data = ordemServico.Id
        };
    }

}
