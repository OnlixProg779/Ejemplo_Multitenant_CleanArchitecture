using Multitenant.Application.Models.LoginService;

namespace Multitenant.Application.Contracts.Services
{
    public interface ILogginService
    {
        Task<AuthResponse> LoginUser(AuthRequest request);
    }
}
