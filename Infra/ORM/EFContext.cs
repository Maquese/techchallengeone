using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Domain.VOs;
using Domain.Entidades;
namespace Infra;

public class EFContext : DbContext
{

    public DbSet<Peca> Pecas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<OrdemServico> OrdemServicos { get; set; }
    public DbSet<Servico> Servicos { get; set; }

    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Peca>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            //entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.DataCadastro).IsRequired();
            entity.Property(e => e.DataAtualizacao).IsRequired();
            entity.Property(e => e.Ativo).IsRequired();
            entity.Property(e => e.QuantidadeEmEstoque).IsRequired();
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(c => c.Veiculos)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteId);

            entity.OwnsOne(c => c.Cpf, cpf =>
            {
                cpf.Property(c => c.Numero)
                .HasColumnName("Cpf")   // coluna no banco
                .HasMaxLength(11)
                .IsRequired();

                // índice único sobre Numero
                cpf.HasIndex(c => c.Numero).IsUnique();
            });

            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Celular).HasMaxLength(20);
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Placa).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Marca).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Ano).IsRequired();
            entity.HasOne(v => v.Cliente)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.ClienteId);

        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.TempoEstimado).IsRequired();
        });

        modelBuilder.Entity<OrdemServico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.HasOne(o => o.Veiculo)
                .WithMany(v => v.OrdemServicos)
                .HasForeignKey(o => o.VeiculoId);

            entity.HasMany(o => o.Servicos)
                .WithMany(s => s.OrdemServicos)
                .UsingEntity(j => j.ToTable("OrdemServicoServicos"));

            entity.HasMany(o => o.Pecas)
                .WithMany(p => p.OrdemServicos)
                .UsingEntity(j => j.ToTable("OrdemServicoPecas"));
        });
    }

}
