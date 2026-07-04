using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
using Domain.VOs;
using Aplication.Interfaces;

namespace Aplication.Services;

public class ClienteAppServiceImp
{
    private readonly ClienteRepository _clienteRepository;
    private readonly VeiculoRepository _veiculoRepository;

    public ClienteAppServiceImp(ClienteRepository clienteRepository, VeiculoRepository veiculoRepository)
    {
        _clienteRepository = clienteRepository;
        _veiculoRepository = veiculoRepository;
    }
    
    #region Veiculo
    public async Task AdicionarVeiculo(VeiculoModel veiculoModel)
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

    public async Task<VeiculoModel> BuscarVeiculo(string placa)
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

    public async Task InativarVeiculo(int id)
    {
        var veiculo = await _veiculoRepository.ObterPorId(id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        await _veiculoRepository.Inativar(veiculo);
    }

    public async Task AtualizarVeiculo(UpdateVeiculoModel veiculoModel)
    {
        var veiculo = await _veiculoRepository.ObterPorId(veiculoModel.Id);
        if (veiculo == null)
        {
            throw new DomainException("Veículo não encontrado");
        }

        veiculo.Atualizar(new PlacaVO(veiculoModel.Placa), veiculoModel.Modelo, veiculoModel.Marca, veiculoModel.Ano);

        await _veiculoRepository.Atualizar(veiculo);
    }

    #endregion
}
