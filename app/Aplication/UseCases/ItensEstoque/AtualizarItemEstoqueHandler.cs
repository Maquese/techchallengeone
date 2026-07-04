using Aplication.Interfaces;
using Aplication.Models;
using Domain.Exceptions;

namespace Aplication.UseCases.ItensEstoque;

public class AtualizarItemEstoqueHandler
{
    private readonly ItemEstoqueRepository _itemEstoqueRepository;

    public AtualizarItemEstoqueHandler(ItemEstoqueRepository itemEstoqueRepository)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

     public async Task Handle(UpdateItemEstoqueModel itemEstoqueModel)
    {
        var itemExistente = await _itemEstoqueRepository.ObterPorId(itemEstoqueModel.Id);
        if (itemExistente == null)
        {
            throw new DomainException("Item de estoque não encontrado para atualização.");
        }

        itemExistente.Atualizar(
            itemEstoqueModel.Nome,
            itemEstoqueModel.Descricao,
            itemEstoqueModel.Valor,
            itemEstoqueModel.Tipo,
            itemEstoqueModel.UnidadeMedida,
            itemEstoqueModel.DataValidade
        );
        await _itemEstoqueRepository.Atualizar(itemExistente);
    }
}
