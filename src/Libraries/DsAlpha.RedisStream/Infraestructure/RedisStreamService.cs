using Hangfire;
using Microsoft.Extensions.Logging;
using Polly;
using StackExchange.Redis;
using System.Text.Json;

namespace DsAlpha.RedisStream.Infraestructure
{

    namespace DsAlpha.RedisStream.Infrastructure
    {
        public interface IRedisStreamService
        {
            Task AddToStreamAsync<T>(T item);
        }

        public class RedisStreamService : IRedisStreamService
        {
            private readonly IDatabase _db;
            private readonly ILogger<RedisStreamService> _logger;
            private readonly IAsyncPolicy _retryPolicy;
            private readonly string _streamName = "RegistrarLogMonitorPbo";
            private readonly long _maxMessagesInStream = 2; // Ejemplo, ajustar según necesidades
            private readonly IBackgroundJobClient _backgroundJobClient;
            private readonly IRecurringJobManager _recurringJobManager;
            public RedisStreamService(
                IConnectionMultiplexer redis, 
                ILogger<RedisStreamService> logger
                , IBackgroundJobClient backgroundJobClient
                , IRecurringJobManager recurringJobManager
                )
            {
                _db = redis.GetDatabase();
                _logger = logger;

                _retryPolicy = Policy.Handle<RedisException>()
                    .Or<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, timespan, retryCount, context) =>
                        {
                            _logger.LogWarning(exception, "Error al interactuar con Redis. Reintentando intento {RetryCount} en {Delay}s", retryCount, timespan.TotalSeconds);
                        });
                _backgroundJobClient = backgroundJobClient;
                _recurringJobManager = recurringJobManager;
                _recurringJobManager.AddOrUpdate("verificar_y_publicar_buffers", () => CheckAndPublishStreamAsync(), Cron.MinuteInterval(10));
            }

            public async Task AddToStreamAsync<T>(T item)
            {
                var jsonData = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _db.StreamAddAsync(_streamName, "data", jsonData);
                    _logger.LogInformation($"Mensaje agregado al stream {_streamName}");
                });

                await CheckAndPublishStreamAsync();
            }

            public async Task CheckAndPublishStreamAsync()
            {
                var length = await _db.StreamLengthAsync(_streamName);
                if (length >= _maxMessagesInStream)
                {
                    await PublishToHangfireJobAsync();
                }
            }

            private async Task PublishToHangfireJobAsync()
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    _backgroundJobClient.Enqueue<StreamProcessingService>(service => service.ProcessStreamJob(_streamName));
                    _logger.LogInformation($"{DateTime.UtcNow} - Job encolado para procesar el stream: {_streamName}");
                });
            }
        }
    }
}
