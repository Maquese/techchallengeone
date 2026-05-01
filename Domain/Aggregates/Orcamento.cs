using Domain.Entidades;

namespace Domain.Aggregates;

public class Orcamento : IEntity
{
    protected Orcamento() { }

    public int Id { get; private set; }
    public int OrdemServicoId { get; private set; }
    public OrdemServico OrdemServico { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string Observacao { get; private set; }
    public bool OrcamentoAprovado { get; private set; }
    public DateTime? DataDecisaoClienteAprovacao { get; private set; }
    public bool OrcamentoPago { get; private set; }
    public DateTime? DataDecisaoClientePagamento { get; private set; }
    

    public Orcamento(int ordemServicoId, decimal valorTotal, string observacoes)
    {
        OrdemServicoId = ordemServicoId;
        ValorTotal = valorTotal;
        Observacao = observacoes;
        OrcamentoAprovado = false;
        DataDecisaoClienteAprovacao = null;
        Ativo = true;
    }

    public void AprovarOrcamento()
    {
        OrcamentoAprovado = true;
        DataDecisaoClienteAprovacao = DateTime.Now;
    }

    public void ReprovarOrcamento()
    {
        OrcamentoAprovado = false;
        DataDecisaoClienteAprovacao = DateTime.Now;
    }

    public void MarcarOrcamentoPago()
    {
        OrcamentoPago = true;
        DataDecisaoClientePagamento = DateTime.Now;
    }
}
