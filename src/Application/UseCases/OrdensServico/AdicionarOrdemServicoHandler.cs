
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.UseCases.OrdensServico;

public class AdicionarOrdemServicoHandler
{
    private readonly VeiculoRepository _veiculoRepository;
    private readonly ServicoRepository _servicoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ILogger<AdicionarOrdemServicoHandler> _logger;

    public AdicionarOrdemServicoHandler(VeiculoRepository veiculoRepository, ServicoRepository servicoRepository, 
                                        OrdemServicoRepository ordemServicoRepository, ILogger<AdicionarOrdemServicoHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _servicoRepository = servicoRepository;
        _ordemServicoRepository = ordemServicoRepository;
        _logger = logger;
    }
    
    public async Task<BaseResponse> Handle(AddOrdemServicoRequest ordemServico)
    {
        var stopwatch = Stopwatch.StartNew();

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["event_type"] = "order_processing",
            ["operation"] = "create_order",
            ["vehicle_id"] = ordemServico.VeiculoId
        });

        _logger.LogInformation("Início da criação da ordem de serviço");

        var veiculo = await _veiculoRepository.ObterPorId(ordemServico.VeiculoId);

        if (veiculo == null)
        {
            _logger.LogWarning(
                "Falha ao criar ordem de serviço: veículo não encontrado");

            throw new DomainException(
                $"Veículo com ID {ordemServico.VeiculoId} não encontrado.");
        }

        var ordemServicoEntity = new OrdemServico(
            ordemServico.VeiculoId,
            ordemServico.ServicosIds != null
                ? await _servicoRepository.ListarPorIds(ordemServico.ServicosIds)
                : new List<Servico>());

        await _ordemServicoRepository.Adicionar(ordemServicoEntity);

        stopwatch.Stop();

        _logger.LogInformation(
            "Ordem de serviço criada com sucesso. OrderId: {OrderId}, Status: {Status}, DurationMs: {DurationMs}",
            ordemServicoEntity.Id,
            ordemServicoEntity.Status,
            stopwatch.ElapsedMilliseconds);

        return new BaseResponse
        {
            Success = true,
            Message = "Ordem de serviço adicionada com sucesso.",
            Data = ordemServicoEntity.Id
        };
    }
}
