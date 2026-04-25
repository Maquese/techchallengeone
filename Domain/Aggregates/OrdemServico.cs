
using Domain.Entidades;

namespace Domain.Aggregates;

public class OrdemServico
{   
    protected OrdemServico() { }

    public int Id { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public string Descricao { get; private set; }
    public int VeiculoId { get; private set; }
    public Veiculo Veiculo { get; private set; }
    public string Status { get; private set; }
    public ICollection<Servico>? Servicos { get; private set; }
    public ICollection<Peca>? Pecas { get; private set; }
    

    public OrdemServico(int id, DateTime dataAbertura, DateTime? dataFechamento,
                        string descricao, int veiculoId, string status, ICollection<Servico>? servicos = null, ICollection<Peca>? pecas = null)
    {
        Id = id;
        DataAbertura = dataAbertura;
        DataFechamento = dataFechamento;
        Descricao = descricao;
        VeiculoId = veiculoId;
        Status = status;
        Servicos = servicos ?? new List<Servico>();
        Pecas = pecas ?? new List<Peca>();
    }
}
