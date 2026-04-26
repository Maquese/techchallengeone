
using Domain.Entidades;

namespace Domain.Aggregates;

public class OrdemServico
{   
    protected OrdemServico() { }

    public int Id { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public int VeiculoId { get; private set; }
    public Veiculo Veiculo { get; private set; }
    public string Status { get; private set; }
    public ICollection<Servico>? Servicos { get; private set; }
    public ICollection<ItemEstoque>? Pecas { get; private set; }
    public string? MecanicoAtribuido { get; private set; }
    

    public OrdemServico(int id, DateTime dataAbertura, DateTime? dataFechamento, 
           int veiculoId, ICollection<Servico>? servicos = null, 
           ICollection<ItemEstoque>? pecas = null, string? mecanicoAtribuido = null)
    {
        Id = id;
        DataAbertura = dataAbertura;
        DataFechamento = dataFechamento;
        VeiculoId = veiculoId;
        Status = "Recebida";
        Servicos = servicos ?? new List<Servico>();
        Pecas = pecas ?? new List<ItemEstoque>();
        MecanicoAtribuido = mecanicoAtribuido;
    }

    public void OSEmDiagnostico(string mecanico)
    {
        MecanicoAtribuido = mecanico;
        Status = "Em Diagnostico";
    }

    public void OSDiagnosticada(IList<ItemEstoque> pecas, IList<Servico> servicos)
    {
        Pecas = pecas;
        Servicos = servicos;
        MecanicoAtribuido = null;
        Status = "Agardando Aprovação";
    }
}
