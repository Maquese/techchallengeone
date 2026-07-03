using Aplication.Services;
using Domain.Interfaces;
using Infra;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<ItemEstoqueAppServiceImp>();
            services.AddTransient<ClienteAppServiceImp>();
            services.AddTransient<OrdemServicoAppServiceImp>();
            services.AddTransient<OrcamentoAppServiceImp>();

            // services.AddTransient<RotaRepository, RotaRepositoryImp>();

            // services.AddTransient<RotaService, RotaServiceImp>();
        }
    }
