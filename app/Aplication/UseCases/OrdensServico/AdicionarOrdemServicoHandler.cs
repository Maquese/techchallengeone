using Aplication.Interfaces;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;

namespace Aplication.UseCases.OrdensServico;

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
    
    public async Task<int> Handle(AddOrdemServicoModel ordemServico)
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
        return ordemServicoEntity.Id;
    }
}
