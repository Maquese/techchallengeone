using Aplication.Interfaces;
using Domain.Aggregates;

namespace Aplication.UseCases.ItensEstoque;

public class ListarItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public ListarItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }    
    public async Task<IEnumerable<ItemEstoque>> Handle()
    {
        return await _itemEstoqueRepository.ListarAtivos();
    }

}
