namespace MultitenantGateway.Api.HelperHandler
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        // Aquí puedes incluir lógica personalizada si es necesario, por ejemplo, modificar solicitudes/respuetas
        // Para este caso, simplemente pasamos la solicitud a través del handler
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
