using Domain.Aggregates;

namespace Domain.Entidades;

public class Servico: IEntity
{
    public Servico( string descricao, decimal valor, int tempoEstimado)
    {
        Descricao = descricao;
        Valor = valor;
        TempoEstimado = tempoEstimado;
         Ativo = true;
    }

    protected Servico() { }
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }   
    public int TempoEstimado { get; private set; }
    public ICollection<OrdemServico> OrdemServicos { get; private set; }

    public void Atualizar(string descricao, decimal valor, int tempoEstimado)
    {
        Descricao = descricao;
        Valor = valor;
        TempoEstimado = tempoEstimado;
    }
}
