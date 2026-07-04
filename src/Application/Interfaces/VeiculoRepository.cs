using Domain.Entidades;

namespace Aplication.Interfaces;

public interface VeiculoRepository : BaseRepository<Veiculo>
{
    Task<Veiculo> BuscarPorPlaca(string placa);
}
