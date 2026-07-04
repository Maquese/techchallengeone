using Aplication.Interfaces;
using Aplication.Models;
using Domain.Aggregates;

namespace Aplication.UseCases.ItensEstoque;

public class AdicionarItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public AdicionarItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

    public async Task<int> Handle(AddItemEstoqueModel itemEstoqueModel)
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
