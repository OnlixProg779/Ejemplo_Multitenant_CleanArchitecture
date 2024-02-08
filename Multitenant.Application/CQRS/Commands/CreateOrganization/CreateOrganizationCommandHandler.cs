using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.CQRS.Commands.CreateOrganization.Resources;
using Multitenant.Application.Helpers;
using Multitenant.Application.Models;
using Multitenant.Domain.Identity;

namespace Multitenant.Application.CQRS.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, CreateOrganizationResponse>
    {
        private readonly ILogger<CreateOrganizationCommandHandler> _logger; 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MyRoleService _roles;
        private readonly IAsyncRepository<Organization> _repositoryOrganization;
        public CreateOrganizationCommandHandler(ILogger<CreateOrganizationCommandHandler> logger, UserManager<IdentityUser> userManager, MyRoleService roles, IAsyncRepository<Organization> repositoryOrganization)
        {
            _logger = logger;
            _userManager = userManager;
            _roles = roles;
            _repositoryOrganization = repositoryOrganization ??
                 throw new ArgumentNullException(nameof(repositoryOrganization));
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

            var user = new IdentityUser { UserName = request.UserName };

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

            _repositoryOrganization.AddEntity(organizationUser);
            await _repositoryOrganization.Complete(respToken);

            var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Rol);
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogInformation("Error al asignar rol al usuario: {Errors}", string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
            }

            return new CreateOrganizationResponse
            {
                Success = true,
                UserId = user.Id
            };
        }
    }
}
