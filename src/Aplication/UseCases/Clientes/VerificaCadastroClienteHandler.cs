using Aplication.Interfaces;
using Aplication.Models;

namespace Aplication.UseCases.Clientes;

public class VerificaCadastroClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public VerificaCadastroClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<UpdateClienteModel> Handle(string documento)
    {
        var cliente = await _clienteRepository.ObterPorDocumento(documento);
        if (cliente == null)
        {
            return null;
        }

        return new UpdateClienteModel
        {
            Id = cliente.Id,
            Documento = cliente.Documento.Numero,
            Nome = cliente.Nome,
            Email = cliente.Email.Endereco,
            Celular = cliente.Celular.Numero
        };
    }
}
