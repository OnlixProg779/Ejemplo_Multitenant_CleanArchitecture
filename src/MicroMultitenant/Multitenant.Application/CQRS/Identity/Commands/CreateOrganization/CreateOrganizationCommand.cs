using MediatR;
using Multitenant.Application.CQRS.Identity.Commands.CreateOrganization.Resources;

namespace Multitenant.Application.CQRS.Identity.Commands.CreateOrganization
{
    public class CreateOrganizationCommand : CreateOrganizationRequest, IRequest<CreateOrganizationResponse>
    {
        public string Rol { get; set; } = null!;
        public string? Token { get; set; }

    }
}
