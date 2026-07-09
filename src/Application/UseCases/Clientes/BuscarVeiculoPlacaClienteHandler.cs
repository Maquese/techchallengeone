using Aplication.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;

namespace Aplication.UseCases.Clientes;

public class BuscarVeiculoPlacaClienteHandler
{
    private readonly VeiculoRepository _veiculoRepository;

    public BuscarVeiculoPlacaClienteHandler(VeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository; 
    }

    public async Task<BaseResponse> Handle(string placa)
    {
        var veiculo = await _veiculoRepository.BuscarPorPlaca(placa);
        if (veiculo == null)
        {
            throw new Exception("Veiculo não encontrado");
        }

        if(!veiculo.EstaAtivo())
            throw new Exception("Veiculo inativo");

        return new BaseResponse
        {
            Message = "Veiculo encontrado com sucesso",
            Success = true,
            Data = new
            {
                Id = veiculo.Id,
                Placa = veiculo.Placa.Valor,
                Modelo = veiculo.Modelo,
                Marca = veiculo.Marca,
                Ano = veiculo.Ano
            }
        };
    }
}
