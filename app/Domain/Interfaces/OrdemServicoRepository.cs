using Domain.Aggregates;
using Domain.Entidades;

namespace Domain.Interfaces;

public interface OrdemServicoRepository : BaseRepository<OrdemServico>
{
    Task<IEnumerable<OrdemServico>> ListarOrdensServicoPorCliente(List<int> veiculosIds);
    Task<List<OrdemServico>> ListarOrdensServicoPorStatus(IList<string> status);
}
