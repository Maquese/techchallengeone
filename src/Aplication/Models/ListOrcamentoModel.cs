namespace Aplication.Models;

public class ListOrcamentoModel
{
    public int OrdemServicoId { get; set; }
    public decimal Valor { get; set; }
    public int Id { get; set; }
    public bool? OrcamentoAprovado { get; set; }
    public bool? OrcamentoPago { get; set; }
    public DateTime DataCadastro { get; set; }
}
