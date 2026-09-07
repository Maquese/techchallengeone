
using Application.Models.Requests;
using Domain.Exceptions;
using Application.Interfaces;
using Application.Models.Responses;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.UseCases.OrdensServico;

public class FinalizarOrdemServicoHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ItemEstoqueRepository _itensEstoqueRepository;
    private readonly ILogger<FinalizarOrdemServicoHandler> _logger;

    public FinalizarOrdemServicoHandler(OrdemServicoRepository ordemServicoRepository, 
                                        ItemEstoqueRepository itensEstoqueRepository,
                                        ILogger<FinalizarOrdemServicoHandler> logger)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _itensEstoqueRepository = itensEstoqueRepository;
        _logger = logger;
    }
    
    public async Task<BaseResponse> Handle(int ordemServicoId)
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
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = Activity.Current?.TraceId.ToString() ?? "n/a",
            ["event_type"] = "order_processing",
            ["operation"] = "finalize_order_execution",
            ["order_id"] = ordemServico.Id,
            ["vehicle_id"] = ordemServico.VeiculoId,
            ["status"] = ordemServico.Status
        });
        await DeduzirItensEstoque(ordemServico.OrdemServicoItensEstoque?.Select(i => new AddItensOrdemServicoRequest { id = i.ItemEstoqueId, quantidade = i.Quantidade }).ToList() ?? new List<AddItensOrdemServicoRequest>());

        ordemServico.FinalizarOrdemServico();
        await _ordemServicoRepository.Atualizar(ordemServico);
        _logger.LogInformation(
            "Ordem de serviço ID {OrderId} finalizada com sucesso. VehicleId: {VehicleId}, Status: {Status}, Data: {Data}",
            ordemServico.Id,
            ordemServico.VeiculoId,
            ordemServico.Status,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        return new BaseResponse{
            Message = $"Ordem de serviço ID {ordemServicoId} finalizada com sucesso.",
            Success = true,
            Data = ordemServico.Id
        };
    }

    private async Task DeduzirItensEstoque(List<AddItensOrdemServicoRequest> itensEstoque)
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
}
