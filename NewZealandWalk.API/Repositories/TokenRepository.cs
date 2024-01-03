using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewZealandWalk.API.Repositories
{
    [Serializable]
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.Email, user.Email));

            roles.ForEach(r => claimList.Add(new Claim(ClaimTypes.Role, r)));

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claimList, null, DateTime.Now.AddMinutes(15), credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
