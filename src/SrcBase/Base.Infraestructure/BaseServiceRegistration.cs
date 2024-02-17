using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Base.Application.Constants;
using Base.Application.Models;
using System.Text;
using Base.Application.Contracts.Repository;
using Base.Infraestructure.Repository;

namespace Base.Infraestructure
{
    public static class BaseServiceRegistration
    {
      
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
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
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

        public static IServiceCollection ConfigureBaseInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            return services;

        }

    }
}
