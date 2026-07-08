using Aplication.Interfaces;
using Application.Models.Requests;
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

     public async Task Handle(AddVeiculoRequest veiculoRequest)
    {
        var veiculo = new Veiculo
        (
            new PlacaVO(veiculoRequest.Placa),
            veiculoRequest.Modelo,
            veiculoRequest.Marca,
            veiculoRequest.Ano,
            veiculoRequest.ClienteId
        );

        await _veiculoRepository.Adicionar(veiculo);
    }

}
