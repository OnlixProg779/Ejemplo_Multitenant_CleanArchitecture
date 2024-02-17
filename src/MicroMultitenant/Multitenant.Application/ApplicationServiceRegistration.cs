using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Application.Behaviors;

namespace Multitenant.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddMultitenantApplicationServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
