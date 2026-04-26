namespace Aplication.Models;

public class ItemEstoqueModel
{
    public int Id { get;  set; }
    public string Tipo { get;  set; }
    public string Nome { get; set; }
    public string Descricao { get;  set; }
    public decimal Valor { get;  set; }
    public DateTime DataCadastro { get;  set; }
    public DateTime DataAtualizacao { get;  set; }
    public DateTime? DataValidade { get;  set; }
    public bool Ativo { get; set; }
}
