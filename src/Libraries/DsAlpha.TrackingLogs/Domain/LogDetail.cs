namespace DsAlpha.TrackingLogs.Domain
{
    public class LogDetail
    {
        // Identificador único para cada entrada de log (opcional)
        public Guid LogId { get; set; } = Guid.NewGuid();

        // Fecha y hora del evento
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Nivel de severidad del log
        public string Level { get; set; }

        // Mensaje descriptivo del evento
        public string Message { get; set; }

        // Nombre de la máquina o servicio donde ocurre el evento
        public string MachineName { get; set; }

        // Nombre del método o función donde se genera el log
        public string MethodName { get; set; }

        // Información adicional que puede ser útil para el diagnóstico
        public object AdditionalInfo { get; set; }

        // Usuario que realiza la acción, si es aplicable
        public string UserId { get; set; }

        // Información sobre cualquier excepción que pueda haber ocurrido
        public Exception Exception { get; set; }

        // Constructor vacío
        public LogDetail() { }

        // Puedes añadir aquí métodos de ayuda o constructores adicionales si es necesario
    }
}
