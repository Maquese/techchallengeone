using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;
using Domain.VOs;

namespace Aplication.UseCases.Clientes;

public class AtualizarVeiculoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public AtualizarVeiculoClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }    

    public async Task Handle(UpdateVeiculoModel veiculoModel)
    {
        var veiculo = await _veiculoRepository.ObterPorId(veiculoModel.Id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        veiculo.Atualizar(new PlacaVO(veiculoModel.Placa), veiculoModel.Modelo, veiculoModel.Marca, veiculoModel.Ano);

        await _veiculoRepository.Atualizar(veiculo);
    }
}
