using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Multitenant.Application.Models
{
    public class MyRoleService
    {
        public List<IdentityRole>? Roles { get; set; }

        /// <summary>
        /// Método para cargar roles
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public async Task LoadRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            Roles = await roleManager.Roles.ToListAsync();
        }
    }
}
