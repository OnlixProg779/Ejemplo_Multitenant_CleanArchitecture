using Multitenant.Application.Exceptions;
using Multitenant.Application.Helpers;
using Multitenant.Helpers;
using Newtonsoft.Json;

namespace Multitenant.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TenantMiddleware> _logger;

        public TenantMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var pathSegments = context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string organizationName = string.Empty;

                int companyIndex = Array.IndexOf(pathSegments, "company");
                if (companyIndex != -1 && pathSegments.Length > companyIndex + 1)
                {
                    organizationName = pathSegments[companyIndex + 1];
                }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    var token = context.Request.Headers["Authorization"].ToString();
                    var respToken = new MyTokenInformation(token);
                    if (organizationName != respToken.OrganizationName)
                    {
                        _logger.LogInformation("No pertenece a la organización: {OrganizationName}", organizationName);
                        throw new Exception("El usuario no pertenece a la organización");
                    }

                    var templateConnectionString = _configuration.GetConnectionString("Bussines");
                    if (templateConnectionString.Contains("[_OrganizationName_]"))
                    {
                        var connectionString = templateConnectionString.Replace("[_OrganizationName_]", organizationName);
                        context.Items["ConnectionStringLoad"] = connectionString;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                int statusCode = 500;
                string result = string.Empty;
                if (!(ex is NotFoundException))
                {
                    if (!(ex is Application.Exceptions.ValidationException validationException))
                    {
                        if (ex is BadRequestException)
                        {
                            statusCode = 400;
                        }
                    }
                    else
                    {
                        statusCode = 400;
                        result = JsonConvert.SerializeObject(new CodeErrorException(details: JsonConvert.SerializeObject(validationException.Errors), statusCode: statusCode, message: ex.Message));
                    }
                }
                else
                {
                    statusCode = 404;
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, ex.Message, ex.StackTrace));
                }

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(result);
            }
        }
    }

}
