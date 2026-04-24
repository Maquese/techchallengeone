using Domain.Aggregates;

namespace Domain.InfraInterfaces;

public interface ClienteRepository : BaseRepository<Cliente>
{
    Cliente ObterPorCpf(string cpf);
}
