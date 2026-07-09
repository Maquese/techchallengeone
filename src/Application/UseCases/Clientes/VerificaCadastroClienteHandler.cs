using Aplication.Interfaces;
using Application.Models.Responses;

namespace Aplication.UseCases.Clientes;

public class VerificaCadastroClienteHandler
{
    private readonly ClienteRepository _clienteRepository;

    public VerificaCadastroClienteHandler(ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<BaseResponse> Handle(string documento)
    {
        var cliente = await _clienteRepository.ObterPorDocumento(documento);
        if (cliente == null)
        {
            throw new Exception("Cliente não encontrado");
        }

        if(!cliente.EstaAtivo())
        {
            throw new Exception("Cliente inativo");
        }

        return new BaseResponse
        {
            Message = "Encontrado com sucesso",
            Success = true,
            Data = cliente.Id
        };
    }
}
