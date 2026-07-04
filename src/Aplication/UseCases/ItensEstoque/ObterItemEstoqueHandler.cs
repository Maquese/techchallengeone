using Aplication.Interfaces;
using Domain.Aggregates;

namespace Aplication.UseCases.ItensEstoque;
public class ObterItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public ObterItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }    
    public async Task<ItemEstoque> Handle(int id)
    {
        return await _itemEstoqueRepository.ObterPorId(id);
    }

}
