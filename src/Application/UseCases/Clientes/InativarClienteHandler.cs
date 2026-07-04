using Aplication.Interfaces;
using Domain.Exceptions;

namespace Aplication.UseCases.Clientes;

public class InativarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public InativarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }   

    public async Task Handle(int id)
    {
        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente == null)
        {
            throw new DomainException("Cliente não encontrado");
        }

        await _clienteRepository.Inativar(cliente);
    }
}
