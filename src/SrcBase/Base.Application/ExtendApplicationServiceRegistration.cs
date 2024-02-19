using Base.Application.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Application
{
    public static class ExtendApplicationServiceRegistration
    {
        public static IServiceCollection AddExtendApplicationServices(this IServiceCollection services)
        {
          

            return services;
        }

        public static IServiceCollection AddRolesServices(this IServiceCollection services)
        {
            services.AddSingleton<MyRoleService>();

            return services;

        }
    }
}
