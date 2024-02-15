namespace Multitenant.Application.CQRS.Commands.CreateOrganization.Resources
{
    public class CreateOrganizationRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;

    }
}
