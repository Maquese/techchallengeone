using Aplication.Models;
using Domain;
using Domain.Aggregates;
using Domain.InfraInterfaces;

namespace Aplication.Services;

public class ItemEstoqueAppServiceImp
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public ItemEstoqueAppServiceImp(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

    public async Task<int> AdicionarItemEstoque(AddItemEstoqueModel itemEstoqueModel)
    {
        var novaPeca = new ItemEstoque
        (
            itemEstoqueModel.Tipo,
            itemEstoqueModel.Nome,
            itemEstoqueModel.Descricao,
            itemEstoqueModel.Valor,
            itemEstoqueModel.UnidadeMedida,
            itemEstoqueModel.DataValidade
        );
        await _itemEstoqueRepository.Adicionar(novaPeca);
        return novaPeca.Id;
    }
}
    