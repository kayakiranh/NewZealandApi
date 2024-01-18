using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewZealandWalk.API.Models.Identity.Domain;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Access token and Refresh token generator repository
    /// </summary>
    [Serializable]
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataProtectionProvider _provider;
        private readonly IDataProtector _dataProtecter;

        public TokenRepository(IConfiguration configuration, UserManager<AppUser> userManager, IDataProtectionProvider provider)
        {
            _configuration = configuration;
            _userManager = userManager;
            _provider = provider;
            _dataProtecter = _provider.CreateProtector(_configuration.GetValue<string>("DataProtecterKey") ?? "Super!Secret1Protecter3Key$");
        }

        public async Task<Tuple<string, string, DateTime>> CreateTokens(AppUser user, List<string> roles)
        {
            List<Claim> claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _dataProtecter.Protect(user.Email))
            };

            roles.ForEach(r => claimList.Add(new Claim(ClaimTypes.Role, _dataProtecter.Protect(r))));

            byte[] secret = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret"));
            DateTime expiration = DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:Expiration"));
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                _configuration.GetValue<string>("Jwt:Issuer"),
                _configuration.GetValue<string>("Jwt:Audience"),
                claimList,
                expires: expiration,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature));

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            await _userManager.SetAuthenticationTokenAsync(user, "NewZealandWalks", "AccessToken", accessToken);

            JwtSecurityToken jwtToken2 = new JwtSecurityToken(
                _configuration.GetValue<string>("Jwt:Issuer"),
                _configuration.GetValue<string>("Jwt:Audience"),
                claimList,
                expires: expiration.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature));

            string refreshToken = new JwtSecurityTokenHandler().WriteToken(jwtToken2);

            return new Tuple<string, string, DateTime>(accessToken, refreshToken, expiration);
        }

        public async Task<Tuple<AppUser, string, string, DateTime>?> CreateNewTokens(string refreshToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(refreshToken);
            Claim emailClaim = securityToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            IEnumerable<Claim> roleClaims = securityToken.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            AppUser? appUser = await _userManager.FindByEmailAsync(_dataProtecter.Unprotect(emailClaim.Value));
            if (appUser == null) return null;

            if (appUser.RefreshTokenExpiration < DateTime.Now) return null;

            List<string> roles = new List<string>();
            roleClaims.ToList().ForEach(x => roles.Add(_dataProtecter.Unprotect(x.Value)));

            Tuple<string, string, DateTime> newTokenData = await CreateTokens(appUser, roles);
            return new Tuple<AppUser, string, string, DateTime>(appUser, newTokenData.Item1, newTokenData.Item2, newTokenData.Item3);
        }
    }
}