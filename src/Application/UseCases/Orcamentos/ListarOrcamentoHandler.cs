using Aplication.Interfaces;
using Aplication.Models;

namespace Aplication.UseCases.Orcamentos;

public class ListarOrcamentoHandler
{
    private readonly OrcamentoRepository _orcamentoRepository;

    public ListarOrcamentoHandler(OrcamentoRepository orcamentoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
    }
    public async Task<List<ListOrcamentoModel>> Handle()
    {
        var orcamentos = await _orcamentoRepository.ListarAtivos();
        return orcamentos.Select(o => new ListOrcamentoModel
        {
            Id = o.Id,
            OrdemServicoId = o.OrdemServicoId,
            Valor = o.ValorTotal,
            OrcamentoAprovado = o.OrcamentoAprovado,
            OrcamentoPago = o.OrcamentoPago,
            DataCadastro = o.DataCadastro
        }).ToList();
    }
}
