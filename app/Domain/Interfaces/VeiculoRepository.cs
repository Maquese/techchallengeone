using Domain.Entidades;

namespace Domain.Interfaces;

public interface VeiculoRepository : BaseRepository<Veiculo>
{
    Task<Veiculo> BuscarPorPlaca(string placa);
}
