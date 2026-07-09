using Aplication.UseCases.Clientes;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.UseCases.OrdensServico;
using Domain.Exceptions;

namespace Application.Controllers;

public class OrdemServicoAppController
{
    private AdicionarClienteHandler _adicionarClienteHandler;
    private AdicionarVeiculoClienteHandler _adicionarVeiculoClienteHandler;
    private AdicionarOrdemServicoHandler _adicionarOrdemServicoHandler;

    public OrdemServicoAppController(AdicionarClienteHandler adicionarClienteHandler, AdicionarVeiculoClienteHandler adicionarVeiculoClienteHandler,
                                     AdicionarOrdemServicoHandler adicionarOrdemServicoHandler)
    {
        _adicionarClienteHandler = adicionarClienteHandler;
        _adicionarVeiculoClienteHandler = adicionarVeiculoClienteHandler;
        _adicionarOrdemServicoHandler = adicionarOrdemServicoHandler;
    }

    public async Task<BaseResponse> AbrirOrdemServico(AberturaOSRequest request)
    {
        var data = await _adicionarClienteHandler.Handle(new AddClienteRequest
        {
            Documento = request.Documento,
            Nome = request.Nome,
            Email = request.Email,
            Celular = request.Celular
        });

        if((int)data.Data <= 0)
        {
            throw new DomainException("Erro ao criar cliente.");
        }

        var dataVeiculo = await _adicionarVeiculoClienteHandler.Handle(new AddVeiculoRequest
        {
            Placa = request.Placa,
            Modelo = request.Modelo,
            Marca = request.Marca,
            Ano = request.Ano,
            ClienteId = (int)data.Data
        });

        if((int)dataVeiculo.Data <= 0)
        {
            throw new DomainException("Erro ao criar veículo.");
        } 

        var dataOrdemServico = await _adicionarOrdemServicoHandler.Handle(new AddOrdemServicoRequest
        {
            ServicosIds = request.ServicosIds,
            VeiculoId = (int)dataVeiculo.Data   
        });       

        if((int)dataOrdemServico.Data <= 0)
        {
            throw new DomainException("Erro ao criar ordem de serviço.");
        }

        

        return dataOrdemServico;
    }
}
