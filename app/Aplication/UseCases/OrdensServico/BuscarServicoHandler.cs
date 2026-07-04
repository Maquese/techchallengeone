using Aplication.Interfaces;
using Aplication.Models;

namespace Aplication.UseCases.OrdensServico;

public class BuscarServicoHandler
{
    private readonly ServicoRepository _servicoRepository;

    public BuscarServicoHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<ServicoModel> Handle(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            return null;
        }

        return new ServicoModel
        {
            Id = servico.Id,
            Descricao = servico.Descricao,
            Valor = servico.Valor,
            TempoEstimado = servico.TempoEstimado
        };
    }

}
