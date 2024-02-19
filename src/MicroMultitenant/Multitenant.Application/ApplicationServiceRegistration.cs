using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Application.Behaviors;
using System.Reflection;

namespace Multitenant.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddMultitenantApplicationServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
