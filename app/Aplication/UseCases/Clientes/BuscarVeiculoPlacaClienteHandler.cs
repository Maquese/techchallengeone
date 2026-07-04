using Aplication.Interfaces;
using Aplication.Models;

namespace Aplication.UseCases.Clientes;

public class BuscarVeiculoPlacaClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public BuscarVeiculoPlacaClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository; 
    }

    public async Task<VeiculoModel> Handle(string placa)
    {
        var veiculo = await _veiculoRepository.BuscarPorPlaca(placa);
        if (veiculo == null)
        {
            return null;
        }

        return new VeiculoModel
        {
            Id = veiculo.Id,
            Placa = veiculo.Placa.Valor,
            Modelo = veiculo.Modelo,
            Marca = veiculo.Marca,
            Ano = veiculo.Ano,
            ClienteId = veiculo.ClienteId
        };
    }
}
