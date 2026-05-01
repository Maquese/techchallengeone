using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Domain.VOs;
using Domain.Entidades;
namespace Infra;

public class EFContext : DbContext
{

    public DbSet<ItemEstoque> ItensEstoque { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<OrdemServico> OrdemServicos { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Orcamento> Orcamentos { get; set; }
    public DbSet<OrdemServicoItemEstoque> OrdemServicoItensEstoque { get; set; }
    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ItemEstoque>(entity =>
        {
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.ToTable("ItemEstoque");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            //entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.DataCadastro).IsRequired();
            entity.Property(e => e.DataAtualizacao).IsRequired();
            entity.Property(e => e.Ativo).IsRequired();
            entity.Property(e => e.QuantidadeEmEstoque).IsRequired();
            entity.Property(e => e.Datavalidade).IsRequired(false);
            entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnidadeMedida).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.ToTable("Cliente");
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
            
            entity.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Endereco)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();

                email.HasIndex(e => e.Endereco).IsUnique();
            });

            entity.OwnsOne(c => c.Celular, celular =>
            {
                celular.Property(c => c.Numero)
                    .HasColumnName("Celular")
                    .HasMaxLength(11)
                    .IsRequired();

                celular.HasIndex(c => c.Numero).IsUnique();
            });
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.ToTable("Veiculo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Marca).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Ano).IsRequired();
            entity.HasOne(v => v.Cliente)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.ClienteId);

                entity.OwnsOne(v => v.Placa, placa =>
                {
                    placa.Property(p => p.Valor)
                        .HasColumnName("Placa")
                        .HasMaxLength(8)
                        .IsRequired();

                    placa.HasIndex(p => p.Valor).IsUnique();
                });
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.ToTable("Servico");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.TempoEstimado).IsRequired();
        });

        modelBuilder.Entity<OrdemServico>(entity =>
        {
            entity.ToTable("OrdemServico");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.MecanicoAtribuido).HasMaxLength(200);
            entity.HasOne(o => o.Veiculo)
                .WithMany(v => v.OrdemServicos)
                .HasForeignKey(o => o.VeiculoId);

            entity.HasMany(o => o.Servicos)
                .WithMany(s => s.OrdemServicos)
                .UsingEntity(j => j.ToTable("OrdemServicoServicos"));
        });

        modelBuilder.Entity<OrdemServicoItemEstoque>(entity =>
        {
            entity.ToTable("OrdemServicoItensEstoque");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Quantidade).IsRequired();
            entity.Property(e => e.DataCadastro).IsRequired();

            entity.HasOne(op => op.OrdemServico)
                .WithMany(o => o.OrdemServicoItensEstoque)
                .HasForeignKey(op => op.OrdemServicoId);

            entity.HasOne(op => op.ItemEstoque)
                .WithMany(ie => ie.OrdemServicoItensEstoque)
                .HasForeignKey(op => op.ItemEstoqueId);

            // Índice para evitar duplicatas
            entity.HasIndex(op => new { op.OrdemServicoId, op.ItemEstoqueId }).IsUnique();
        });

        modelBuilder.Entity<Orcamento>(entity =>
        {
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.ToTable("Orcamento");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ValorTotal).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Observacao).HasMaxLength(1000);
            entity.Property(e => e.OrcamentoAprovado).IsRequired(false);
            entity.Property(e => e.DataDecisaoClienteAprovacao).IsRequired(false);
            entity.Property(e => e.OrcamentoPago).IsRequired(false);
            entity.Property(e => e.DataDecisaoClientePagamento).IsRequired(false);
            entity.Property(e => e.DataCadastro).IsRequired();
            entity.HasOne(o => o.OrdemServico)
                .WithMany(os => os.Orcamentos)
                .HasForeignKey(o => o.OrdemServicoId);
        });
    }
}
