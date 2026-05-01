
using Domain.Entidades;

namespace Domain.Aggregates;

public class OrdemServico : IEntity
{   
    protected OrdemServico() { }

    public int Id { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public int VeiculoId { get; private set; }
    public Veiculo Veiculo { get; private set; }
    public string Status { get; private set; }
    public ICollection<Servico>? Servicos { get; private set; }
    public ICollection<ItemEstoque>? ItensEstoque { get; private set; }
    public string? MecanicoAtribuido { get; private set; }
    public IList<Orcamento> Orcamentos { get; private set; }
    public DateTime? DataInicioExecucao { get; private set; }
    public DateTime? DataFimExecucao { get; private set; }
    

    public OrdemServico(DateTime dataAbertura, DateTime? dataFechamento, 
           int veiculoId, ICollection<Servico>? servicos = null, 
           ICollection<ItemEstoque>? pecas = null, string? mecanicoAtribuido = null)
    {
        DataAbertura = dataAbertura;
        DataFechamento = dataFechamento;
        VeiculoId = veiculoId;
        Status = "Recebida";
        Servicos = servicos ?? new List<Servico>();
        ItensEstoque = pecas ?? new List<ItemEstoque>();
        MecanicoAtribuido = mecanicoAtribuido;
        Orcamentos = new List<Orcamento>();
        Ativo = true;
    }

    public void OSEmDiagnostico(string mecanico)
    {
        MecanicoAtribuido = mecanico;
        Status = "Em Diagnostico";
    }

    public void OSDiagnosticada(IList<ItemEstoque> itens, IList<Servico> servicos)
    {
        ItensEstoque = itens;
        Servicos = servicos;
        MecanicoAtribuido = null;
        Status = "Aguardando Aprovação";
    }

    public void OSAprovada()
    {
        Status = "Aprovada";
    }

    public void EmExecucao(string mecanico)
    {
        MecanicoAtribuido = mecanico;
        Status = "Em Execução";
        DataInicioExecucao = DateTime.Now;
    }

    public void FinalizarOrdemServico()
    {
        Status = "Finalizada";
        DataFimExecucao = DateTime.Now;
    }

    public void OrdemServicoEntregue()
    {
        Status = "Entregue";
        DataFechamento = DateTime.Now;
    }
}
