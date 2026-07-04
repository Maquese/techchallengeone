using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;

namespace Aplication.UseCases.OrdensServico;

public class AtualizarServicoHandler
{
    private readonly ServicoRepository _servicoRepository;

    public AtualizarServicoHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }
    
    public async Task Handle(UpdateServicoModel servicoModel)
    {
        var servico = await _servicoRepository.ObterPorId(servicoModel.Id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        servico.Atualizar(servicoModel.Descricao, servicoModel.Valor, servicoModel.TempoEstimado);

        await _servicoRepository.Atualizar(servico);
    }

}
