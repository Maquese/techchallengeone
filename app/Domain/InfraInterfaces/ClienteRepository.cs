using Domain.Aggregates;

namespace Domain.InfraInterfaces;

public interface ClienteRepository : BaseRepository<Cliente>
{
    Task<Cliente> ObterPorDocumento(string documento);
}
