
using Domain.Exceptions;
using Application.Interfaces;
using Application.Models.Responses;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.UseCases.Orcamentos;

public class PagarOrcamentoHandler
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ILogger<PagarOrcamentoHandler> _logger;

    public PagarOrcamentoHandler(OrcamentoRepository orcamentoRepository, 
                                 OrdemServicoRepository ordemServicoRepository,
                                 ILogger<PagarOrcamentoHandler> logger)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse> Handle(int orcamentoId)
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
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = Activity.Current?.TraceId.ToString() ?? "n/a",
            ["event_type"] = "order_processing",
            ["operation"] = "finalize_order",
            ["order_id"] = ordemServico.Id,
            ["vehicle_id"] = ordemServico.VeiculoId,
            ["status"] = ordemServico.Status
        });

        if(ordemServico.Status != "Finalizada")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Orçamento Aprovado' para pagamento de orçamento.");
        }

        orcamento.MarcarOrcamentoPago();
        ordemServico.OrdemServicoEntregue();
        await _orcamentoRepository.Atualizar(orcamento);
        _logger.LogInformation(
            "Orçamento ID {OrcamentoId} pago com sucesso. OrderId: {OrderId}, VehicleId: {VehicleId}, Status: {Status}, Data: {Data}",
            orcamento.Id,
            ordemServico.Id,
            ordemServico.VeiculoId,
            ordemServico.Status,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        return new BaseResponse
        {
            Success = true, 
            Message = "Orcamento pago com sucesso",
            Data = orcamento.Id
        };
    }
}
