using Domain.InfraInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class BaseRepositoryImp<T> : BaseRepository<T> where T : class
{
    protected readonly EFContext _context;
    public BaseRepositoryImp(EFContext context)
    {
        _context = context;
    }

    
    public void Adicionar(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public void Atualizar(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public void Inativar(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public List<T> ListarAtivos()
    {
        return _context.Set<T>().ToList();
    }

    public T? ObterPorId(int id)
    {
        return _context.Set<T>().Find(id);
    }
}
