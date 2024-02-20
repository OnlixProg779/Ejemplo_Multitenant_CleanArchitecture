using Base.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

        public static IServiceCollection ConfigureRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de Redis
            var redisConfiguration = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

            // configurar opciones específicas para Redis:
            // services.AddSingleton<IConnectionMultiplexer>(sp =>
            // {
            //     var configuration = ConfigurationOptions.Parse(redisConfiguration);
            //     // Modifica la configuración según sea necesario
            //     return ConnectionMultiplexer.Connect(configuration);
            // });

            return services;
        }
    }
}
