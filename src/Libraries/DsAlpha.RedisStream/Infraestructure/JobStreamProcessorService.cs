using Polly;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using DsAlpha.RedisStream.Application.Contracts;

namespace DsAlpha.RedisStream.Infraestructure
{

    public class JobStreamProcessorService : IJobStreamProcessorService
    {
        public readonly IDatabase _redisDb;
        private readonly ILogger<JobStreamProcessorService> _logger;
        public IAsyncPolicy _retryPolicy;

        public JobStreamProcessorService(IConnectionMultiplexer redis, ILogger<JobStreamProcessorService> logger)
        {
            _redisDb = redis.GetDatabase();
            _logger = logger;
            ConfigureRetryPolicy();
        }

        private void ConfigureRetryPolicy()
        {
            _retryPolicy = Policy
                .Handle<RedisConnectionException>() // errores de conexión
                .Or<RedisTimeoutException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Error al procesar stream {context["streamName"]}, reintentando {retryCount} vez. Error: {exception.Message}");
                    });
        }

        public virtual Task ProcessStreamJob(string streamName)
        {
            var context = new Context();
            context["streamName"] = streamName;

            return _retryPolicy.ExecuteAsync(async (ctx) =>
            {
                var length = await _redisDb.StreamLengthAsync(streamName);
                var streamEntries = await _redisDb.StreamReadAsync(streamName, "0-0", (int)length);
                _logger.LogInformation($"{DateTime.UtcNow} - Procesando stream {streamName} con {streamEntries.Length} entradas.");
            }, context);
        }
    }

}
