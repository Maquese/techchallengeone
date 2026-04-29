using Aplication.Services;
using Domain.InfraInterfaces;
using Infra;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IOC;

 public static class BootStrapper
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddDbContext<EFContext>(x => x.UseInMemoryDatabase("EFContext"));

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
