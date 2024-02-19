using MediatR;
using Multitenant.Application.CQRS.Business.Commands.InitOrganization.Resources;

namespace Multitenant.Application.CQRS.Business.Commands.InitOrganization
{
    public class InitOrganizationCommand: IRequest<InitOrganizationResponse>
    {
        public string OrganizationName { get; set; } = null!;
        public string? Token { get; set; }
    }
}
