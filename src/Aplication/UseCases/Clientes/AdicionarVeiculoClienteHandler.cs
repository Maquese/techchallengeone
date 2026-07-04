using Aplication.Interfaces;
using Aplication.Models;
using Domain.Entidades;
using Domain.VOs;

namespace Aplication.UseCases.Clientes;

public class AdicionarVeiculoClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;
    private readonly ClienteRepository _clienteRepository;
    public AdicionarVeiculoClienteHandler(VeiculoRepository veiculoRepository, ClienteRepository clienteRepository)
    {
        _veiculoRepository = veiculoRepository;
        _clienteRepository = clienteRepository;
    }

     public async Task Handle(VeiculoModel veiculoModel)
    {
        var veiculo = new Veiculo
        (
            new PlacaVO(veiculoModel.Placa),
            veiculoModel.Modelo,
            veiculoModel.Marca,
            veiculoModel.Ano,
            veiculoModel.ClienteId
        );

        await _veiculoRepository.Adicionar(veiculo);
    }

}
