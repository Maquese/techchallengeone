using Aplication.Interfaces;
using Domain.Exceptions;

namespace Aplication.UseCases.ItensEstoque;

public class InativarItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public InativarItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }    
    public async Task Handle(int id)
    {
        var itemExistente = await _itemEstoqueRepository.ObterPorId(id);
        if (itemExistente == null)
        {
            throw new DomainException("Item de estoque não encontrado para inativação.");
        }

        itemExistente.Inativar();
        await _itemEstoqueRepository.Atualizar(itemExistente);
    }

}
