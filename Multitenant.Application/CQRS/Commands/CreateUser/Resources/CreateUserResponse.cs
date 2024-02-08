namespace Multitenant.Application.CQRS.Commands.CreateUser.Resources
{
    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
