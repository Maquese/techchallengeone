using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;
using Domain.VOs;

namespace Aplication.UseCases.Clientes;

public class AtualizarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public AtualizarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(UpdateClienteModel clienteModel)
    {
        var cliente = await _clienteRepository.ObterPorId(clienteModel.Id);
        if (cliente == null)
        {
            throw new DomainException("Cliente não encontrado");
        }
        try
        {
            cliente.AtualizarComDocumento(
                new DocumentoVO(clienteModel.Documento),
                clienteModel.Nome,
                new EmailVO(clienteModel.Email),
                new CelularVO(clienteModel.Celular)
            );

            await _clienteRepository.Atualizar(cliente);
        }
        catch (DomainException ex)
        {
            throw ex;
        }
    }
}
