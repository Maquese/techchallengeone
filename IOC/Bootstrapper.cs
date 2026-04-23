using Domain;
using Domain.Aggregates;
using Domain.InfraInterfaces;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IOC;

 public static class BootStrapper
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddDbContext<EFContext>(x => x.UseInMemoryDatabase("EFContext"));

            services.AddTransient<PecaRepository, PecaRepositoryImp>();

            services.AddTransient<PecaService>();

            // services.AddTransient<RotaRepository, RotaRepositoryImp>();

            // services.AddTransient<RotaService, RotaServiceImp>();
        }
    }
