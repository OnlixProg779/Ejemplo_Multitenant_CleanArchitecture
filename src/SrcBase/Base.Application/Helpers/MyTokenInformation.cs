
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Base.Application.Helpers
{
    public class MyTokenInformation
    {
        //public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? OrganizationName { get; set; }
        public Guid IdUserApplication { get; set; }

        public List<Claim> Claims { get; set; }

        public MyTokenInformation(string token)
        {
            this.getAttributesToken(token);
        }

        private void getAttributesToken(string token)
        {

            var tokenAux = token.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenAux);

            Claims = jwtSecurityToken.Claims.ToList();
            IdUserApplication = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);
            Email = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
            Role = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            OrganizationName = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value;
        }


    }
}
