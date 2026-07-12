
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Application.Interfaces; 

namespace Application.UseCases.OrdensServico;

public class AdicionarOrdemServicoHandler
{
    private readonly VeiculoRepository _veiculoRepository;
    private ServicoRepository _servicoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public AdicionarOrdemServicoHandler(VeiculoRepository veiculoRepository, ServicoRepository servicoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _veiculoRepository = veiculoRepository;
        _servicoRepository = servicoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }
    
    public async Task<BaseResponse> Handle(AddOrdemServicoRequest ordemServico)
    {
        var veiculo = await _veiculoRepository.ObterPorId(ordemServico.VeiculoId);
        if (veiculo == null)        {
            throw new DomainException($"Veículo com ID {ordemServico.VeiculoId} não encontrado.");
        }

        var ordemServicoEntity = new OrdemServico(
            ordemServico.VeiculoId,
            servicos: ordemServico.ServicosIds != null 
                ? await _servicoRepository.ListarPorIds(ordemServico.ServicosIds) 
                : new List<Servico>()
        );
        await _ordemServicoRepository.Adicionar(ordemServicoEntity); 
        return new BaseResponse
        {
            Success = true,
            Message = "Ordem de serviço adicionada com sucesso.",
            Data = ordemServicoEntity.Id
        };
    }
}
