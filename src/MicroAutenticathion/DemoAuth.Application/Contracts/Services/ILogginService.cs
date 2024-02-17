using DemoAuth.Application.Models.LoginService;

namespace DemoAuth.Application.Contracts.Services
{
    public interface ILogginService
    {
        Task<AuthResponse> LoginUser(AuthRequest request);
    }
}
