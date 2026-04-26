using Domain.Aggregates;

namespace Domain.Entidades;

public class Insumo
{
    public int Id { get; private set; }
    public string Nome { get;private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public bool Ativo { get; private set; }
    public int QuantidadeEmEstoque { get; private set; }
}
