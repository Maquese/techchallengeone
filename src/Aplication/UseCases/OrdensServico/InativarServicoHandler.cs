using Aplication.Interfaces;
using Domain.Exceptions;

namespace Aplication.UseCases.OrdensServico;

public class InativarServicoHandler
{
    private readonly ServicoRepository _servicoRepository;

    public InativarServicoHandler(ServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task Handle(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        await _servicoRepository.Inativar(servico);
    }
}
