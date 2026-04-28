using Domain.Entidades;

namespace Domain.Aggregates;

public class Orcamento : IEntity
{
    protected Orcamento() { }

    public int Id { get; private set; }
    public int OrdemServicoId { get; private set; }
    public OrdemServico OrdemServico { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string Observacoes { get; private set; }

    public Orcamento(int id, int ordemServicoId, decimal valorTotal, string observacoes)
    {
        Id = id;
        OrdemServicoId = ordemServicoId;
        ValorTotal = valorTotal;
        Observacoes = observacoes;
    }
}
