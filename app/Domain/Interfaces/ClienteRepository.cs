using Domain.Aggregates;

namespace Domain.Interfaces;

public interface ClienteRepository : BaseRepository<Cliente>
{
    Task<Cliente> ObterPorDocumento(string documento);
}
