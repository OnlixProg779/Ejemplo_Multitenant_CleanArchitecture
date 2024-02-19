using Newtonsoft.Json;

namespace MultitenantGateway.Api.Middlewares
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TestMiddleware> _logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
            }
        }
    }

}
