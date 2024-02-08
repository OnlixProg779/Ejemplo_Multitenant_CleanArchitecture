using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Multitenant.Application.Constants;
using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Models;
using Multitenant.Infraestructure.Persistence;
using Multitenant.Infraestructure.Repository.Generic;
using System.Text;

namespace Multitenant.Infraestructure
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<IdentityOrganizationDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("IdentityOrganization"), 
               b => b.MigrationsAssembly(typeof(IdentityOrganizationDbContext).Assembly.FullName)
               ));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityOrganizationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 3;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.SignIn.RequireConfirmedEmail = false;

            });

            return services;

        }

        public static IServiceCollection ConfigureBussinesServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<BussinesDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("Bussines"),
               b => b.MigrationsAssembly(typeof(BussinesDbContext).Assembly.FullName)
               ));

            return services;

        }

        public static IServiceCollection AddExtendJwtServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings")); 

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
                //ValidIssuer = configuration["JwtSettings:Issuer"],
                //ValidAudience = configuration["JwtSettings:Audience"],
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;

            });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("Organization", p =>
                                   p.RequireRole(CustomRoles.Organizacion));
                cfg.AddPolicy("UserOrganization", p =>
                                   p.RequireRole(CustomRoles.OrganizacionUsuario));
                cfg.AddPolicy("Manager", p => 
                                   p.RequireRole(CustomRoles.AdminPro));
            });

            return services;

        }

        public static IServiceCollection ConfigureInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            return services;

        }

    }
}
