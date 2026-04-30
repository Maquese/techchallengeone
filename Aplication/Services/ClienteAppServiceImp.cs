using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
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
            Email = cliente.Email.Endereco,
            Celular = cliente.Celular.Numero
        };
    }

    public async Task<int> CriarCliente(ClienteModel clienteModel)
    {
        try
        {
            var cliente = new Cliente
            (
                new CpfVO(clienteModel.Cpf),
                clienteModel.Nome,
                new EmailVO(clienteModel.Email),
                new CelularVO(clienteModel.Celular)
            );

            await _clienteRepository.Adicionar(cliente);
            return cliente.Id;
        }
        catch (DomainException ex)
        {
            throw ex;
        }
    }

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
}
