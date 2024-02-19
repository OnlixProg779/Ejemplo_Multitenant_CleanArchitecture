using Microsoft.Extensions.Logging;

namespace DsAlpha.TrackingLogs.Test
{
    public class TestHandler
    {
        private readonly ILogger<TestHandler> _logger;

        public TestHandler(ILogger<TestHandler> logger)
        {
            _logger = logger;
        }

        public async Task CrearTiposDeLogs()
        {
            // Trace y debug solo se deben logear en tiempo de desarrollo y en el monitor:
            _logger.LogTrace($"CrearTiposDeLogs - TRACE");
            _logger.LogDebug($"CrearTiposDeLogs - DEBUG");

            // En tiempo de produccion el log de debug se va a guardar en un archivo pero solo si se lo activa (deberia haber algun switch en el appsetting.json que indique si queremos guardar los logs de debug o si no queremos hacerlo)
            _logger.LogDebug($"CrearTiposDeLogs - DEBUG");

            // Log information se va a guardar en una base de datos
            _logger.LogInformation($"CrearTiposDeLogs - Informatio");

            // En tiempo de produccion el log de warning se va a guardar en la misma base de datos de log information pero solo si se lo activa (deberia haber algun switch en el appsetting.json que indique si queremos guardar los logs de warning o si no queremos hacerlo)
            _logger.LogWarning($"CrearTiposDeLogs - Warning");

            // Siempre se van a guardar en la base de datos los logs de error
            _logger.LogError($"CrearTiposDeLogs - ERROR");

            // Siempre se van a guardar en la base de datos los logs de critical y tambien en un archivo
            _logger.LogCritical($"CrearTiposDeLogs - CRITICAL");

        }
    }
}
