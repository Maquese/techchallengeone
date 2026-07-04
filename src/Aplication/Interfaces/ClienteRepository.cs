using Domain.Aggregates;

namespace Aplication.Interfaces;

public interface ClienteRepository : BaseRepository<Cliente>
{
    Task<Cliente> ObterPorDocumento(string documento);
}
