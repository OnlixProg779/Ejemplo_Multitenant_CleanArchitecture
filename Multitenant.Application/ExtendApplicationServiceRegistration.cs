using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Application.Behaviors;
using Multitenant.Application.Models;
using System.Reflection;

namespace Multitenant.Application
{
    public static class ExtendApplicationServiceRegistration
    {
        public static IServiceCollection AddExtendApplicationServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        public static async Task LoadRolesAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var myRoleService = services.GetRequiredService<MyRoleService>();
            await myRoleService.LoadRolesAsync(roleManager);
        }
    }
}
