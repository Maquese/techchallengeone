using Aplication.Interfaces;
using Aplication.Models;
using Domain.Entidades;

namespace Aplication.UseCases.OrdensServico;

public class AdicionarServicoHandler
{
    private readonly ServicoRepository _servicoRepository;

    public AdicionarServicoHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository; 
    }
     public async Task<ServicoModel> Handle(ServicoModel servico)
    {
        var servicoEntity = new Servico
        (
            descricao: servico.Descricao,
            valor: servico.Valor,
            tempoEstimado: servico.TempoEstimado
        );
        
        await _servicoRepository.Adicionar(servicoEntity);
        return servico;
    }
}
