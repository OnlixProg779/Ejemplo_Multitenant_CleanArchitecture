﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using DemoAuth.Domain.Identity;
using Base.Application.Constants;

namespace DemoAuth.Infraestructure.Persistence
{
    public class IdentityOrganizationSeedData
    {
        public static async Task SeedAsync(IdentityOrganizationDbContext _context, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IdentityOrganizationSeedData>();

            if (!_roleManager.Roles!.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(CustomRoles.AdminPro));
                await _roleManager.CreateAsync(new IdentityRole(CustomRoles.Organizacion));
                await _roleManager.CreateAsync(new IdentityRole(CustomRoles.OrganizacionUsuario));
                logger.LogInformation("Estamos insertando nuevos records al Rol Context de identidad");

            }

            if (!_userManager.Users!.Any()) 
            {
                var user = GetPreconfiguredUser();
                var result = await _userManager.CreateAsync(user, "fxDxxFhj.!21");
                logger.LogInformation("Estamos insertando nuevos records al context de identidad");

                if (result.Succeeded)
                {
                    var applicationUser = new Organization
                    {
                        //Id = new Guid("5c389b4c-3f81-4cab-b7cc-29ff29f01c9e"),
                        IdentityUserId = user.Id,
                        OrganizationName = "MyOrganization",
                    };

                    _context.OrganizationIdentity!.Add(applicationUser);
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, CustomRoles.AdminPro);

                }
            }
        }

        private static IdentityUser GetPreconfiguredUser()
        {

            return new IdentityUser
            {
                //Id = new Guid("565d2ea3-3915-4369-84b5-77a20db72b5c").ToString(),
                Email = "ds@pruebas.net",
                UserName = "ds@pruebas.net",
                EmailConfirmed = true,
            };

        }
    }
}
