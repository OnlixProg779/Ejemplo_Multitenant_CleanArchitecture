using DsAlpha.RedisStream.Application.Contracts;
using Hangfire;
using Microsoft.Extensions.Logging;
using Polly;
using StackExchange.Redis;
using System.Text.Json;

namespace DsAlpha.RedisStream.Infraestructure
{

    namespace DsAlpha.RedisStream.Infrastructure
    {

        public class JobInitOrganizationService : IJobInitOrganizationService
        {
            private readonly IDatabase _db;
            private readonly ILogger<JobInitOrganizationService> _logger;
            private readonly IAsyncPolicy _retryPolicy;
            private readonly IBackgroundJobClient _backgroundJobClient;
            public JobInitOrganizationService(
                IConnectionMultiplexer redis,
                ILogger<JobInitOrganizationService> logger
                , IBackgroundJobClient backgroundJobClient
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
            }

            public async Task AddToStreamAsync<T>(T item)
            {
                var jsonData = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = false });
                var streamName = Guid.NewGuid().ToString();

                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _db.StreamAddAsync(streamName, "data", jsonData);
                    _logger.LogInformation($"Mensaje agregado al stream {streamName}");
                });

                await PublishToHangfireJobAsync(streamName);
            }

            private async Task PublishToHangfireJobAsync(string jobStreamName)
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    _backgroundJobClient.Enqueue<IJobStreamProcessorService>(service => service.ProcessStreamJob(jobStreamName));
                    _logger.LogInformation($"{DateTime.UtcNow} - Job encolado para procesar el stream: {jobStreamName}");
                });
            }
        }
    }
}
