
namespace Multitenant.Application.CQRS.Commands.CreateUser.Resources
{
    public class CreateUserRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
