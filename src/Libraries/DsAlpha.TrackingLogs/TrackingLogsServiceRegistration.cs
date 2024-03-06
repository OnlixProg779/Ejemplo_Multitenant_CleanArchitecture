using DsAlpha.TrackingLogs.Application.Contracts.SerilogSink;
using DsAlpha.TrackingLogs.Infraestructure.SerilogSink;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Base.Application
{
    public static class TrackingLogsServiceRegistration
    {
        public static IServiceCollection ConfigureSerilogServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddSingleton<IFileFatalLogEventSink, FileFatalLogEventSink>();
            services.AddSingleton<IFileDebugLogEventSink, FileDebugLogEventSink>();
            services.AddSingleton<IDatabaseLogEventSink, DatabaseLogEventSink>();

            return services;

        }

        public static IHostBuilder UseSerilogWithConfiguration(this IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                var logConfig = context.Configuration.GetSection("Logging:SwitchLog");
                var logDebugToFile = logConfig.GetValue<bool>("LogDebugToFile");
                var logWarningToDatabase = logConfig.GetValue<bool>("LogWarningToDatabase");

                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName();

                if (context.HostingEnvironment.IsDevelopment())
                {
                    configuration.WriteTo.Console();
                }
                else
                // Activar el log de Debug en archivo basado en la configuración
                if (logDebugToFile)
                {
                    configuration.WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Debug)
                        .WriteTo.Sink(new FileDebugLogEventSink()));
                }

                // Configurar el log de Warning a la base de datos si está activado
                if (logWarningToDatabase)
                {
                    configuration.WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Warning)
                        .WriteTo.Sink(new DatabaseLogEventSink()));
                }

                // loguea siempre Information Error y Fatal en la base de datos
                configuration.WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Information || evt.Level == Serilog.Events.LogEventLevel.Error || evt.Level == Serilog.Events.LogEventLevel.Fatal)
                        .WriteTo.Sink(new DatabaseLogEventSink()));

                // loguea siempre falta en un archivo
                configuration.WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Fatal)
                        .WriteTo.Sink(new FileFatalLogEventSink()));

                configuration.WriteTo.Sink(new FileFatalLogEventSink(), Serilog.Events.LogEventLevel.Fatal);
            });

            return builder;
        }

    }
}
