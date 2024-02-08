namespace Multitenant.Application.CQRS.Commands.CreateOrganization.Resources
{
    public class CreateOrganizationResponse
    {
        public bool Success { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
