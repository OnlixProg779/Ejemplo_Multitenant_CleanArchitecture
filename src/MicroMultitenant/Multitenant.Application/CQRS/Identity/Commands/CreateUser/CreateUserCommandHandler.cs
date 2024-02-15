using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Multitenant.Application.Contracts.Repository;
using Multitenant.Application.CQRS.Identity.Commands.CreateUser.Resources;
using Multitenant.Application.Helpers;
using Multitenant.Application.Models;
using Multitenant.Domain.Identity;

namespace Multitenant.Application.CQRS.Identity.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MyRoleService _roles;
        private readonly IUnitOfWorkIdentity _repositoryOrganization;
        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, UserManager<IdentityUser> userManager, MyRoleService roles, IUnitOfWorkIdentity repositoryOrganization)
        {
            _logger = logger;
            _userManager = userManager;
            _roles = roles;
            _repositoryOrganization = repositoryOrganization ??
                 throw new ArgumentNullException(nameof(repositoryOrganization));
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var respToken = new MyTokenInformation(request.Token);

            if (request.OrganizationName != respToken.OrganizationName)
            {
                _logger.LogInformation("No pertenece a la organizacion: {OrganizationName}", request.OrganizationName);
                return new CreateUserResponse
                {
                    Success = false,
                    Errors = new List<string> { $"El nombre de la organizacion '{request.OrganizationName}' no es consistente." }
                };
            }

            if (!_roles.Roles.Any(r => r.Name.Equals(request.Rol, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("El rol especificado no existe: {Rol}", request.Rol);
                return new CreateUserResponse
                {
                    Success = false,
                    Errors = new List<string> { $"El rol especificado '{request.Rol}' no existe." }
                };
            }

            var user = new IdentityUser { UserName = request.UserName, Email = request.UserName, EmailConfirmed = true };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogInformation("Error al crear el usuario: {Errors}", string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                return new CreateUserResponse
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

            return new CreateUserResponse
            {
                Success = true,
                UserId = user.Id
            };
        }
    }
}
