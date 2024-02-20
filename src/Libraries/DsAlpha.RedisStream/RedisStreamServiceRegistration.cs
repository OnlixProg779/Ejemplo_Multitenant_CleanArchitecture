using DsAlpha.RedisStream.Infraestructure;
using DsAlpha.RedisStream.Infraestructure.DsAlpha.RedisStream.Infrastructure;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DsAlpha.RedisStream
{
    public static class RedisStreamServiceRegistration
    {
        public static IServiceCollection ConfigureHangfireClienteServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(configuration.GetConnectionString("RedisConnection"), new RedisStorageOptions() 
                {
                    // Opciones de configuración específicas de Redis
                    // Ejemplo: Db = número de base de datos de Redis, si necesitas especificarlo
                    Prefix = "hangfire:", // Prefijo utilizado para todas las claves de Hangfire en Redis
                    // Configuraciones adicionales según sea necesario
                })
            );

            return services;

        }

        public static IServiceCollection ConfigureHangfireServerServices(this IServiceCollection services)
        {
            services.AddHangfireServer();
            return services;
        }

        public static IServiceCollection ConfigurePublishRedisHangfireServices(this IServiceCollection services)
        {
            services.AddSingleton<IRedisStreamService, RedisStreamService>();
            services.AddSingleton<IStreamProcessingService, StreamProcessingService>();

            return services;
        }

    }
}
