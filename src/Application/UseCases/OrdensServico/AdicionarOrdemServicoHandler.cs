
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
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["correlation_id"] = Activity.Current?.TraceId.ToString() ?? "n/a",
            ["event_type"] = "order_processing",
            ["operation"] = "create_order",
            ["vehicle_id"] = ordemServico.VeiculoId,
            ["status"] = "pending"
        });

        _logger.LogInformation(
            "Início da criação da ordem de serviço. VehicleId: {VehicleId}",
            ordemServico.VeiculoId);

        var veiculo = await _veiculoRepository.ObterPorId(ordemServico.VeiculoId);

        if (veiculo == null)
        {
            _logger.LogWarning(
                "Falha ao criar ordem de serviço: veículo não encontrado. VehicleId: {VehicleId}",
                ordemServico.VeiculoId);

            throw new DomainException(
                $"Veículo com ID {ordemServico.VeiculoId} não encontrado.");
        }

        var ordemServicoEntity = new OrdemServico(
            ordemServico.VeiculoId,
            ordemServico.ServicosIds != null
                ? await _servicoRepository.ListarPorIds(ordemServico.ServicosIds)
                : new List<Servico>());

        await _ordemServicoRepository.Adicionar(ordemServicoEntity);

        _logger.LogInformation(
            "Ordem de serviço criada com sucesso. OrderId: {OrderId}, VehicleId: {VehicleId}, Status: {Status}, Data de abertura {Data}",
            ordemServicoEntity.Id,
            ordemServicoEntity.VeiculoId,
            ordemServicoEntity.Status,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        return new BaseResponse
        {
            Success = true,
            Message = "Ordem de serviço adicionada com sucesso.",
            Data = ordemServicoEntity.Id
        };
    }
}
