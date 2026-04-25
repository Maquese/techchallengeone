namespace Domain.InfraInterfaces;

public interface BaseRepository<T> where T : class
{
    Task Adicionar(T entity);
    Task Atualizar(T entity);
    Task Inativar(T entity);
    Task<List<T>> ListarAtivos();
    Task<T> ObterPorId(int id);
}
