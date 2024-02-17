using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.Contracts.Services;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository;
using Multitenant.Infraestructure.Services;

namespace Multitenant.Infraestructure
{
    public static class MultitenantServiceRegistration
    {
        public static IServiceCollection ConfigureMultitenantBussinesServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BusinessDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("Bussines"),
               b => b.MigrationsAssembly(typeof(BusinessDbContext).Assembly.FullName)
               ), ServiceLifetime.Scoped);

            return services;
        }


        public static IServiceCollection ConfigureInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IUnitOfWorkBusiness), typeof(UnitOfWorkBusiness));
            services.AddScoped(typeof(IApplyBusinessMigrations), typeof(ApplyBusinessMigrations));

            return services;

        }

    }
}
