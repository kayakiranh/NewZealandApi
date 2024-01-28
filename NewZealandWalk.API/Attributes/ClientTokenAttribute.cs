using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewZealandWalk.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ClientTokenAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtectionProvider _provider;
        private readonly IDataProtector _dataProtecter;

        public ClientTokenAttribute(ILogger logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _provider = provider;
            _dataProtecter = _provider.CreateProtector(_configuration.GetValue<string>("DataProtecterKey") ?? "Super!Secret1Protecter3Key$");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string? configClientId = _configuration.GetValue<string>("Client:Id");
                string? configClientSecret = _configuration.GetValue<string>("Client:Secret");
                if (string.IsNullOrEmpty(configClientId) || string.IsNullOrEmpty(configClientSecret))
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();

                string? authToken = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authToken))
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(authToken);
                Claim emailClaim = securityToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                IEnumerable<Claim> roleClaims = securityToken.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

                string decyrptedId = _dataProtecter.Unprotect(emailClaim.Value);
                string decyrptedPassword = _dataProtecter.Unprotect(emailClaim.Value);

                bool isIdEqual = string.Equals(configClientId, decyrptedId, StringComparison.CurrentCultureIgnoreCase);
                bool isPasswordEqual = string.Equals(configClientId, decyrptedId, StringComparison.CurrentCultureIgnoreCase);

                if (!isIdEqual || !isPasswordEqual)
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("ClientTokenAttribute Exception {ex}", ex);
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}