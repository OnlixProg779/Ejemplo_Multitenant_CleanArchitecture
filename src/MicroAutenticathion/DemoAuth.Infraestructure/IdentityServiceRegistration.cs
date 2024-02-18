using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DemoAuth.Application.Contracts.Repository;
using DemoAuth.Application.Contracts.Services;
using DemoAuth.Infraestructure.Persistence;
using DemoAuth.Infraestructure.Repository;
using DemoAuth.Infraestructure.Services;

namespace DemoAuth.Infraestructure
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureDemoAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityOrganizationDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("IdentityOrganization"), 
               b => b.MigrationsAssembly(typeof(IdentityOrganizationDbContext).Assembly.FullName)
               ));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityOrganizationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 3;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.SignIn.RequireConfirmedEmail = false;

            });

            return services;

        }

        public static IServiceCollection ConfigureInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IUnitOfWorkIdentity), typeof(UnitOfWorkIdentity));
            services.AddTransient(typeof(ILogginService), typeof(LogginService));

            return services;

        }

    }
}
