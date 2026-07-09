using Aplication.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;

namespace Application.UseCases.ItensEstoque;

public class AdicionarQtdEstoqueItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public AdicionarQtdEstoqueItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

    public async Task Handle(AddQuantidadeItemEstoqueRequest adicionarQuantidadeModel)
    {
        var itemExistente = await _itemEstoqueRepository.ObterPorId(adicionarQuantidadeModel.Id);
        if (itemExistente == null)
        {
            throw new DomainException("Item de estoque não encontrado para adicionar quantidade.");
        }

        itemExistente.AdicionarQuantidadeEstoque(adicionarQuantidadeModel.Quantidade);
        await _itemEstoqueRepository.Atualizar(itemExistente);
    }
}
