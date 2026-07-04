using Aplication.Interfaces;
using Domain.Exceptions;

namespace Aplication.UseCases.Clientes;

public class InativarVeiculoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public InativarVeiculoClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }   

    public async Task Handle(int id)
    {
        var veiculo = await _veiculoRepository.ObterPorId(id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        await _veiculoRepository.Inativar(veiculo);
    }
}
