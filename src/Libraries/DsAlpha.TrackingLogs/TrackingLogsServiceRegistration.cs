using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Base.Application
{
    public static class TrackingLogsServiceRegistration
    {
        public static IServiceCollection AddExtendApplicationServices(this IServiceCollection services, IConfiguration config)
        {
          

            return services;
        }

        public static IHostBuilder UseSerilogWithConfiguration(this IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                // Aquí configuras Serilog basándote en el contexto de hosting y la configuración
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName();

                // Puedes añadir condiciones basadas en el entorno, por ejemplo:
                if (context.HostingEnvironment.IsDevelopment())
                {
                    // Configuraciones específicas para el entorno de desarrollo
                    configuration.WriteTo.Console();
                }
                else
                {
                    // Configuraciones para otros entornos, como producción
                    configuration.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day);
                }

                // Añade aquí más configuraciones de Serilog si es necesario
            });

            return builder;
        }
    }
}
