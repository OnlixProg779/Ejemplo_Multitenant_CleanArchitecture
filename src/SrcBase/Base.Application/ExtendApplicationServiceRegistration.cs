using Base.Application.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Base.Application
{
    public static class ExtendApplicationServiceRegistration
    {
        public static IServiceCollection AddExtendApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(
               cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddRolesServices(this IServiceCollection services)
        {
            services.AddSingleton<MyRoleService>();

            return services;

        }
    }
}
