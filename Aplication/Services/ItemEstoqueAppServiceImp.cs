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

    public void AddPeca(ItemEstoqueModel itemEstoqueModel)
    {
        var novaPeca = new ItemEstoque
        (
            0,
            "Peça",
            itemEstoqueModel.Nome,
            itemEstoqueModel.Descricao,
            itemEstoqueModel.Valor
        );
        _itemEstoqueRepository.Adicionar(novaPeca);
    }
}
    