using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Multitenant.Application.Contracts.Repository.Generic;
using Multitenant.Application.Contracts.Services;
using Multitenant.Application.Models;
using Multitenant.Application.Models.LoginService;
using Multitenant.Domain.Identity;
using Multitenant.Infraestructure.Specification;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Multitenant.Infraestructure.Services
{
    public class LogginService: ILogginService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAsyncRepository<Organization> _repositoryOrganization;

        public LogginService(
           UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           IOptions<JwtSettings> jwtSettings,
           IAsyncRepository<Organization> repositoryOrganization
           )
        {
            _userManager = userManager ??
           throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ??
           throw new ArgumentNullException(nameof(signInManager));
            _jwtSettings = jwtSettings.Value ??
           throw new ArgumentNullException(nameof(jwtSettings));
            _repositoryOrganization = repositoryOrganization ??
        throw new ArgumentNullException(nameof(repositoryOrganization));
        }
        public async Task<AuthResponse> LoginUser(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception($"El usuario con email {request.Email} no existe");
            }
            var resultado = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {
                throw new Exception($"Las credenciales son incorrectas");
            }
            var userRole = await _userManager.GetRolesAsync(user);
            string token = await GenerateTokenToAdminCommand(user, userRole.FirstOrDefault());

            var authResponse = new AuthResponse
            {
                Id = user.Id,
                Token = token,
                Email = user.Email,
                Username = user.UserName
            };

            return authResponse;
        }

        private async Task<string> GenerateTokenToAdminCommand(IdentityUser user, string rol)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var symmertricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var userClaims = await _userManager.GetClaimsAsync(user!);
            var roles = await _userManager.GetRolesAsync(user!);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            List<Expression<Func<Organization, bool>>> criteria = new List<Expression<Func<Organization, bool>>>();
            criteria.Add(a => a.IdentityUserId == user!.Id);
            var spec = new BaseSpecification<Organization>(criteria);

            var entityAppUser = await _repositoryOrganization.GetFirstWithSpec(spec);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, entityAppUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, entityAppUser.OrganizationName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, rol),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                    }.Union(userClaims).Union(roleClaims)),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
                SigningCredentials = new SigningCredentials(symmertricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
