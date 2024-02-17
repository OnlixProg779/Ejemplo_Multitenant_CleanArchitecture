using MediatR;
using DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization.Resources;

namespace DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization
{
    public class CreateOrganizationCommand : CreateOrganizationRequest, IRequest<CreateOrganizationResponse>
    {
        public string Rol { get; set; } = null!;
        public string? Token { get; set; }

    }
}
