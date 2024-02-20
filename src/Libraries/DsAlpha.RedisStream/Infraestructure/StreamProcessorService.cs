using Polly;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace DsAlpha.RedisStream.Infraestructure
{
    public interface IStreamProcessingService
    {
        Task ProcessStreamJob(string streamName);
    }

    public class StreamProcessingService : IStreamProcessingService
    {
        private readonly IDatabase _redisDb;
        private readonly ILogger<StreamProcessingService> _logger;
        private IAsyncPolicy _retryPolicy;

        public StreamProcessingService(IConnectionMultiplexer redis, ILogger<StreamProcessingService> logger)
        {
            _redisDb = redis.GetDatabase();
            _logger = logger;
            ConfigureRetryPolicy();
        }

        private void ConfigureRetryPolicy()
        {
            _retryPolicy = Policy
                .Handle<RedisConnectionException>() // Suponiendo que los errores transitorios son de conexión
                .Or<RedisTimeoutException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Error al procesar stream {context["streamName"]}, reintentando {retryCount} vez. Error: {exception.Message}");
                    });
        }

        public Task ProcessStreamJob(string streamName)
        {
            // Crear un nuevo contexto y agregar datos a él
            var context = new Context();
            context["streamName"] = streamName;

            return _retryPolicy.ExecuteAsync(async (ctx) =>
            {
                // Ahora puedes acceder a ctx["streamName"] dentro de esta lambda si es necesario
                var streamEntries = await _redisDb.StreamReadAsync(streamName, "0-0", -1);
                // Procesar los datos aquí...
                foreach (var entry in streamEntries) { 
                Console.WriteLine(entry);
                }
                _logger.LogInformation($"Procesando stream {streamName} con {streamEntries.Length} entradas.");
            }, context);
        }
    }

}
