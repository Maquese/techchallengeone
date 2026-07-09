using Aplication.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;

namespace Application.UseCases.OrdensServico;

public class FinalizarOrdemServicoHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ItemEstoqueRepository _itensEstoqueRepository;

    public FinalizarOrdemServicoHandler(OrdemServicoRepository ordemServicoRepository, ItemEstoqueRepository itensEstoqueRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _itensEstoqueRepository = itensEstoqueRepository;
    }
    
    public async Task<string> Handle(int ordemServicoId)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(ordemServicoId);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServicoId} não encontrada.");
        }

        if (ordemServico.Status != "Em execução")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Em execução' para finalização.");
        }

        await DeduzirItensEstoque(ordemServico.OrdemServicoItensEstoque?.Select(i => new AddItensOrdemServicoRequest { id = i.ItemEstoqueId, quantidade = i.Quantidade }).ToList() ?? new List<AddItensOrdemServicoRequest>());

        ordemServico.FinalizarOrdemServico();
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Ordem de serviço ID {ordemServicoId} finalizada com sucesso.";
    }

    private async Task DeduzirItensEstoque(List<AddItensOrdemServicoRequest> itensEstoque)
    {   
        foreach (var item in itensEstoque)
        {
            var itemEstoque = await _itensEstoqueRepository.ObterPorId(item.id);
            if (itemEstoque == null)
            {
                throw new DomainException($"Item de estoque com ID {item.id} não encontrado.");
            }

            itemEstoque.DeduzirQuantidadeEstoque(item.quantidade);
            await _itensEstoqueRepository.Atualizar(itemEstoque);
        }
    }
}
