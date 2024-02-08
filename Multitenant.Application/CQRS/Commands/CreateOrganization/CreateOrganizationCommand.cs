using MediatR;
using Multitenant.Application.CQRS.Commands.CreateOrganization.Resources;

namespace Multitenant.Application.CQRS.Commands.CreateOrganization
{
    public class CreateOrganizationCommand: CreateOrganizationRequest, IRequest<CreateOrganizationResponse>
    {
        public string Rol { get; set; } = null!;
        public string? Token { get; set; }

    }
}
