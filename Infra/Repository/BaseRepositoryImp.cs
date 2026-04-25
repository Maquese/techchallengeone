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

    
    public async Task Adicionar(T entity)
    {
       await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Inativar(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> ListarAtivos()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> ObterPorId(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}
