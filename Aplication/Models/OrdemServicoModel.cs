namespace Aplication.Models;

public class OrdemServicoModel
{
    public int? Id { get; set; }
    public string Descricao { get; set; }
    public int VeiculoId { get; set; }
    public string Status { get; set; }
    public List<int> ServicosIds { get; set; }
    public List<int> PecasIds { get; set; }
}
