
using Aplication.Interfaces;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
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

    public async Task<int> Handle(AddClienteModel clienteModel)
    {
        try
        {
            var cliente = new Cliente
            (
                new DocumentoVO(clienteModel.Documento),
                clienteModel.Nome,
                new EmailVO(clienteModel.Email),
                new CelularVO(clienteModel.Celular)
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
