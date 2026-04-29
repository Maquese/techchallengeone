using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.InfraInterfaces;
using Domain.VOs;

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

    public ClienteModel VerificaCadastroCliente(string cpf)
    {
        var cliente = _clienteRepository.ObterPorCpf(cpf);
        if (cliente == null)
        {
            return null;
        }

        return new ClienteModel
        {
            Id = cliente.Id,
            Cpf = cliente.Cpf.Numero,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Celular = cliente.Celular
        };
    }

    public Task<int> CriarCliente(ClienteModel clienteModel)
    {
        var cliente = new Cliente
        (
            new CpfVO(clienteModel.Cpf),
            clienteModel.Nome,
            clienteModel.Email,
            clienteModel.Celular
        );

        _clienteRepository.Adicionar(cliente);
        return Task.FromResult(cliente.Id);
    }

    public Task AdicionarVeiculo(VeiculoModel veiculoModel)
    {
        var veiculo = new Veiculo
        (
            veiculoModel.Placa,
            veiculoModel.Modelo,
            veiculoModel.Marca,
            veiculoModel.Ano,
            veiculoModel.ClienteId
        );

        _veiculoRepository.Adicionar(veiculo);
        return Task.CompletedTask;
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
            Placa = veiculo.Placa,
            Modelo = veiculo.Modelo,
            Marca = veiculo.Marca,
            Ano = veiculo.Ano,
            ClienteId = veiculo.ClienteId
        };
    }
}
