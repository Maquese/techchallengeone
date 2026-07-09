using Aplication.Interfaces;
using Application.Models.Responses;

namespace Aplication.UseCases.OrdensServico;

public class BuscarServicoHandler
{
    private readonly ServicoRepository _servicoRepository;

    public BuscarServicoHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<BaseResponse> Handle(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            return null;
        }

        return new BaseResponse
        {
           Message = "Buscado com sucesso",
           Success = true,
           Data = new ServicoResponse
            {
                Id = servico.Id,
                Descricao = servico.Descricao,
                Valor = servico.Valor,
                TempoEstimado = servico.TempoEstimado
            }
        };
    }
}
