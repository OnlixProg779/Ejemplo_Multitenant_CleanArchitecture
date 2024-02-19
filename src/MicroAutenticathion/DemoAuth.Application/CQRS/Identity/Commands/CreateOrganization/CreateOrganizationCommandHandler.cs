using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using DemoAuth.Application.Contracts.Repository;
using DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization.Resources;
using DemoAuth.Domain.Identity;
using Base.Application.Models;
using Base.Application.Helpers;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, CreateOrganizationResponse>
    {
        private readonly ILogger<CreateOrganizationCommandHandler> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MyRoleService _roles;
        private readonly IUnitOfWorkIdentity _repositoryOrganization;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CreateOrganizationCommandHandler
            (
                ILogger<CreateOrganizationCommandHandler> logger, 
                UserManager<IdentityUser> userManager, 
                MyRoleService roles, 
                IUnitOfWorkIdentity repositoryOrganization,
                IHttpClientFactory httpClientFactory,
                IConfiguration configuration
            )
        {
            _logger = logger;
            _userManager = userManager;
            _roles = roles;
            _configuration = configuration;
            _repositoryOrganization = repositoryOrganization ?? throw new ArgumentNullException(nameof(repositoryOrganization));
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CreateOrganizationResponse> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var respToken = new MyTokenInformation(request.Token);

            if (!_roles.Roles.Any(r => r.Name.Equals(request.Rol, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("El rol especificado no existe: {Rol}", request.Rol);
                return new CreateOrganizationResponse
                {
                    Success = false,
                    Errors = new List<string> { $"El rol especificado '{request.Rol}' no existe." }
                };
            }

            // TODO: Validar que la organizacion no exista antes de empezar las tareas

            var user = new IdentityUser { UserName = request.UserName, Email = request.UserName, EmailConfirmed = true };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogInformation("Error al crear el usuario: {Errors}", string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                return new CreateOrganizationResponse
                {
                    Success = false,
                    Errors = createUserResult.Errors.Select(e => e.Description).ToList()
                };
            }

            var organizationUser = new Organization
            {
                IdentityUserId = user.Id,
                OrganizationName = request.OrganizationName,
            };

            _repositoryOrganization.Repository<Organization>().AddEntity(organizationUser);
            await _repositoryOrganization.Complete(respToken);

            var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Rol);
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogInformation("Error al asignar rol al usuario: {Errors}", string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
            }

            // TODO: Crear evento para creacion de tenant
            await CallMicroserviceBusinessAsync(request.OrganizationName, request.Token[7..],cancellationToken);
      
            return new CreateOrganizationResponse
            {
                Success = true,
                UserId = user.Id
            };
        }

        private async Task CallMicroserviceBusinessAsync(string organizationName, string token, CancellationToken cancellationToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ClientConCertificadoIgnorado");

                string url = $"{_configuration.GetSection("ServiciosRest:Business:Url").Value}{organizationName}";
                string timeoutString = _configuration.GetSection("ServiciosRest:Business:TimeOut").Value;

                TimeSpan timeout = TimeSpan.Parse(timeoutString);

                var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.Timeout = timeout;

                var response = await client.PostAsync(url, emptyContent, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error: {response.StatusCode}, Detalles: {errorResponse}");
                }
                else
                {
                    _logger.LogError($"Error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Error en la solicitud HTTP: {e.Message}");
            }
            catch (Exception)
            {

                throw;
            }
        
        }
    }
}
