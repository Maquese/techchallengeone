using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class EFContext : DbContext
{

    DbSet<Peca> Pecas { get; set; }

    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {
    }
}
