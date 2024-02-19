using DsAlpha.TrackingLogs.Application.Contracts.SerilogSink;
using Serilog.Events;

namespace DsAlpha.TrackingLogs.Infraestructure.SerilogSink
{
    public class FileDebugLogEventSink : IFileDebugLogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}
