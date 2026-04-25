using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Aplication.Services;

public class OrdemServicoAppServiceImp
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ServicoRepository _servicoRepository;

    public OrdemServicoAppServiceImp(OrdemServicoRepository ordemServicoRepository, ServicoRepository servicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _servicoRepository = servicoRepository;
    }

    public async Task<OrdemServico> AdicionarOrdemServico(OrdemServicoModel ordemServico)
    {
        var ordemServicoEntity = new OrdemServico(
            id: ordemServico.Id ?? 0,
            dataAbertura: DateTime.Now,
            dataFechamento: null,
            descricao: ordemServico.Descricao,
            veiculoId: ordemServico.VeiculoId,
            status: ordemServico.Status
        );

        await _ordemServicoRepository.Adicionar(ordemServicoEntity);
        return ordemServicoEntity;
    }

    public async Task<ServicoModel> AdicionarServico(ServicoModel servico)
    {
        var servicoEntity = new Servico
        (
            id: servico.Id ?? 0,
            descricao: servico.Descricao,
            valor: servico.Valor,
            tempoEstimado: servico.TempoEstimado
        );

        await _servicoRepository.Adicionar(servicoEntity);
        return servico;
    }
}
