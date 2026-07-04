using Aplication.Interfaces;
using Aplication.Models;

namespace Aplication.UseCases.OrdensServico;

public class ListarServicosAtivosHandler
{
    private readonly ServicoRepository _servicoRepository;

    public ListarServicosAtivosHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }
    public async Task<List<ServicoModel>> Handle()
    {
        var servicos = await _servicoRepository.ListarAtivos();
        return servicos.Select(s => new ServicoModel
        {
            Id = s.Id,
            Descricao = s.Descricao,
            Valor = s.Valor,
            TempoEstimado = s.TempoEstimado
        }).ToList();
    }

}
