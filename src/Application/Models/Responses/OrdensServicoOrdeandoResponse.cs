namespace Application.Models.Responses;

public class OrdensServicoOrdeandoResponse
{
    public string Status { get; set; }
    public string quantidade { get; set; }
    public List<int> OrdemServicoIds { get; set; }
}
