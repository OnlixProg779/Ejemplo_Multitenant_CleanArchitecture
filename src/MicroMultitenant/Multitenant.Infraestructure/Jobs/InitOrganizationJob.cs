using DsAlpha.RedisStream.Infraestructure;
using MediatR;
using Microsoft.Extensions.Logging;
using Multitenant.Application.CQRS.Business.Commands.InitOrganization;
using StackExchange.Redis;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Multitenant.Infraestructure.Jobs
{
    public class InitOrganizationJob : JobStreamProcessorService
    {
        private readonly ILogger<InitOrganizationJob> _logger;
        private readonly IMediator _mediator;

        public InitOrganizationJob(IConnectionMultiplexer redis,
            IMediator mediator, ILogger<InitOrganizationJob> loggerThis, ILogger<JobStreamProcessorService> logger) : base(redis, logger)
        {
            _mediator = mediator ??
               throw new ArgumentNullException(nameof(mediator));
            _logger = loggerThis;
        }

        public override async Task ProcessStreamJob(string streamName)
        {
            try
            {
                var streamEntries = await ReadStreamEntriesAsync(streamName);
                var commands = DeserializeStreamEntries(streamEntries);

                await ProcessCommand(commands);

                await DeleteStreamIfNotEmpty(streamName, streamEntries.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error procesando el stream {streamName}: {ex.Message}");
                throw; // Considera manejar la excepción de manera que no interrumpa el flujo de la aplicación si es adecuado.
            }
        }

        private async Task<StreamEntry[]> ReadStreamEntriesAsync(string streamName)
        {
            long length = await _redisDb.StreamLengthAsync(streamName);
            return await _redisDb.StreamReadAsync(streamName, "0-0", (int)length);
        }

        private List<InitOrganizationCommand> DeserializeStreamEntries(StreamEntry[] streamEntries)
        {
            var commands = new List<InitOrganizationCommand>();

            foreach (var entry in streamEntries)
            {
                var dataField = entry.Values.FirstOrDefault(v => v.Name == "data");
                if (!dataField.Equals(default(NameValueEntry)))
                {
                    var command = JsonSerializer.Deserialize<InitOrganizationCommand>(dataField.Value);
                    if (command != null)
                    {
                        commands.Add(command);
                    }
                }
            }

            return commands;
        }

        private async Task ProcessCommand(List<InitOrganizationCommand> commands)
        {
            foreach (var item in commands)
            {
                var VMresponse = await _mediator.Send(item);
            }
        }

        private async Task DeleteStreamIfNotEmpty(string streamName, long entriesLength)
        {
            if (entriesLength > 0)
            {
                bool wasDeleted = await _redisDb.KeyDeleteAsync(streamName);
                if (wasDeleted)
                {
                    _logger.LogInformation($"El stream {streamName} ha sido eliminado exitosamente después del procesamiento.");
                }
                else
                {
                    _logger.LogWarning($"Falló la eliminación del stream {streamName}.");
                }
            }
        }

    }
}
