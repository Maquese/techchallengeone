using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;

namespace Aplication.UseCases.OrdensServico;

public class StatusAtualOSClienteHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ClienteRepository _clienteRepository;

    public StatusAtualOSClienteHandler(OrdemServicoRepository ordemServicoRepository, ClienteRepository clienteRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<IList<StatusOSsClienteModel>> Handle(int clienteId)
    {
        var cliente = await _clienteRepository.ObterPorId(clienteId);
        if (cliente == null)        {
            throw new DomainException($"Cliente com ID {clienteId} não encontrado.");
        }
        var ordensServico = await _ordemServicoRepository.ListarOrdensServicoPorCliente(cliente.Veiculos.Select(v => v.Id).ToList());
        return ordensServico.Select(os => new StatusOSsClienteModel
        {
            Id = os.Id,
            Status = os.Status,
            DataCriacao = os.DataAbertura,
            PlacaVeiculo = os.Veiculo.Placa.Valor
        }).ToList();
    }

}
