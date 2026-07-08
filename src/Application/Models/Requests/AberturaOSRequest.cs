using Aplication.Models;

namespace Application.Models.Requests;

public class AberturaOSRequest
{
     public int VeiculoId { get; set; }
    public List<int> ServicosIds { get; set; }
    
}
