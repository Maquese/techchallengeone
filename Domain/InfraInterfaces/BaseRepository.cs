namespace Domain.InfraInterfaces;

public interface BaseRepository<T> where T : class
{
    void Adicionar(T entity);
    void Atualizar(T entity);
    void Inativar(T entity);
    List<T> ListarAtivos();
    T? ObterPorId(int id);
}
