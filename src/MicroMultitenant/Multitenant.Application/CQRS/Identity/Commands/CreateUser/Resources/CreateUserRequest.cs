namespace Multitenant.Application.CQRS.Identity.Commands.CreateUser.Resources
{
    public class CreateUserRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
