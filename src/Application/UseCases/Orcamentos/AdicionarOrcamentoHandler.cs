using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Aggregates;
using Domain.Exceptions;

namespace Application.UseCases.Orcamentos;

public class AdicionarOrcamentoHandler
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public AdicionarOrcamentoHandler(OrcamentoRepository orcamentoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }
    
    public async Task<int> Handle(AddOrcamentoRequest model)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(model.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {model.OrdemServicoId} não encontrada.");
        }
        var orcamento = new Orcamento
        (model.OrdemServicoId, 100,"obs");
        await _orcamentoRepository.Adicionar(orcamento);
        return orcamento.Id;
    }
}
