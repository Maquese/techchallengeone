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
            return null;
        }

        return new BaseResponse
        {
            Message = "Atualizado com sucesso",
            Success = true,
            Data = cliente.Id
        };
    }
}
