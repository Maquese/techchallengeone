using Aplication.Models;
using Domain;
using Domain.Aggregates;
using Domain.Exceptions;
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

    public async Task<IEnumerable<ItemEstoque>> ListarItensEstoque()
    {
        return await _itemEstoqueRepository.ListarAtivos();
    }

    public async Task<ItemEstoque> ObterItemEstoque(int id)
    {
        return await _itemEstoqueRepository.ObterPorId(id);
    }

    public async Task AtualizarItemEstoque(UpdateItemEstoqueModel itemEstoqueModel)
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

    public async Task InativarItemEstoque(int id)
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
    