using Domain.Aggregates;

namespace Domain.InfraInterfaces;

public interface ClienteRepository : BaseRepository<Cliente>
{
    Cliente ObterPorDocumento(string documento);
}
