using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;

namespace Aplication.UseCases.ItensEstoque;

public class AdicionarQtdEstoqueItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public AdicionarQtdEstoqueItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

    public async Task Handle(AddQuantidadeItemEstoqueModel adicionarQuantidadeModel)
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
