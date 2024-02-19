using DsAlpha.TrackingLogs.Application.Contracts.SerilogSink;
using Serilog.Events;

namespace DsAlpha.TrackingLogs.Infraestructure.SerilogSink
{
    public class DatabaseLogEventSink : IDatabaseLogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}
