using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Domain.InfraInterfaces;

public interface VeiculoRepository : BaseRepository<Veiculo>
{
    Task<Veiculo> BuscarPorPlaca(string placa);
}
