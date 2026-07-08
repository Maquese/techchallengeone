
using Aplication.Interfaces;
using Application.Models.Requests;
using Domain.Aggregates;
using Domain.Exceptions;
using Domain.VOs;


namespace Aplication.UseCases.Clientes;

public class AdicionarClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public AdicionarClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<int> Handle(AddClienteRequest clienteRequest)
    {
        try
        {
            var cliente = new Cliente
            (
                new DocumentoVO(clienteRequest.Documento),
                clienteRequest.Nome,
                new EmailVO(clienteRequest.Email),
                new CelularVO(clienteRequest.Celular)
            );

            await _clienteRepository.Adicionar(cliente);
            return cliente.Id;
        }
        catch (DomainException ex)
        {
            throw ex;
        }
    }
}
