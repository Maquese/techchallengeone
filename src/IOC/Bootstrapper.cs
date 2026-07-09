using Aplication.Services;
using Aplication.Interfaces;
using Infra;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aplication.UseCases.Clientes;
using Aplication.UseCases.Orcamentos;
using Application.UseCases.OrdensServico;
using Application.UseCases.Orcamentos;
using Application.Controllers;

namespace IOC;

 public static class BootStrapper
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuthService>();

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"]
                ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("A connection string 'DefaultConnection' must be configured.");
            }
            
            services.AddDbContext<EFContext>(options =>
                options.UseMySQL(connectionString));

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();
            dbContext.Database.Migrate();


            services.AddTransient<ItemEstoqueRepository, ItemEstoqueRepositoryImp>();
            services.AddTransient<ClienteRepository, ClienteRepositoryImp>();
            services.AddTransient<VeiculoRepository, VeiculoRepositoryImp>();
            services.AddTransient<OrdemServicoRepository, OrdemServicoRepositoryImp>();
            services.AddTransient<ServicoRepository, ServicoRepositoryImp>();
            services.AddTransient<OrcamentoRepository, OrcamentoRepositoryImp>();

            services.AddScoped<AdicionarOrcamentoHandler>();
            services.AddScoped<AprovarOrcamentoHandler>();
            services.AddScoped<PagarOrcamentoHandler>();
            services.AddScoped<ListarOrcamentoHandler>();
            services.AddScoped<AdicionarClienteHandler>();
            services.AddScoped<AdicionarVeiculoClienteHandler>();            
            services.AddScoped<AtualizarClienteHandler>();
            services.AddScoped<AtualizarVeiculoClienteHandler>();
            services.AddScoped<BuscarVeiculoPlacaClienteHandler>();
            services.AddScoped<InativarClienteHandler>();
            services.AddScoped<InativarVeiculoClienteHandler>();
            services.AddScoped<VerificaCadastroClienteHandler>();
            services.AddScoped<AdicionarOrdemServicoHandler>();

            services.AddScoped<ConsutaStatusOSHandler>();
            services.AddScoped<NegarOrcamentoHandler>();
            services.AddScoped<ListarOrdensServicoOrdenadoHandler>();

            services.AddTransient<OrdemServicoAppController>();

            // services.AddTransient<RotaRepository, RotaRepositoryImp>();

            // services.AddTransient<RotaService, RotaServiceImp>();
        }
    }
