using Aplication.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
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

     public async Task<BaseResponse> Handle(AddVeiculoRequest veiculoRequest)
    {
        var cliente  = await _clienteRepository.ObterPorId(veiculoRequest.ClienteId);

        if(cliente == null)
            throw new Exception("Cliente não encontrado");

        if (!cliente.EstaAtivo())
            throw new Exception ("Cliente inativo");


        var veiculo = new Veiculo
        (
            new PlacaVO(veiculoRequest.Placa),
            veiculoRequest.Modelo,
            veiculoRequest.Marca,
            veiculoRequest.Ano,
            veiculoRequest.ClienteId
        );

        await _veiculoRepository.Adicionar(veiculo);
        return new BaseResponse
        {
            Success = true,
            Message = "Veículo adicionado com sucesso.",
            Data = veiculo.Id
        };
    }

}
