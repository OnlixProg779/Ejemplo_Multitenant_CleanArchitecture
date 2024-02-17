namespace Base.Helpers
{
    public class CodeErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public CodeErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        private string GetDefaultMessageStatusCode(int statusCode)
        {

            string result = statusCode switch
            {
                400 => "la solicitud enviada contiene errores",
                401 => "No tiene autorizacion para la ejecucion del endpoint",
                404 => "No se encontro el recurso solicitado",
                500 => "Errores en el servidor",
                _ => string.Empty,
            };

            return result;
        }
    }

}
