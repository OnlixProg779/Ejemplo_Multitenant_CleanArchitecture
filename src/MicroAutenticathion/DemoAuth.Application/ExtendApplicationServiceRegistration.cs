using Base.Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAuth.Application
{
    public static class DemoAuthApplicationRegister
    {
        public static async Task LoadRolesAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var myRoleService = services.GetRequiredService<MyRoleService>();
            await myRoleService.LoadRolesAsync(roleManager);
        }
    }
}
