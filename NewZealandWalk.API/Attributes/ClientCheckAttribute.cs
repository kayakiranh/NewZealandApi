using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace NewZealandWalk.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ClientCheckAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientCheckAttribute(ILogger logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

                authToken = authToken.Replace("Basic ", "");
                byte[] data = Convert.FromBase64String(authToken);
                string decodedAuth = Encoding.UTF8.GetString(data);
                string[] authParts = decodedAuth.Split(":");

                string clientId = authParts[0];
                string clientSecret = authParts[1];

                bool isClientIdSame = string.Equals(configClientId, clientId, StringComparison.CurrentCultureIgnoreCase);
                bool isClientSecretSame = string.Equals(configClientSecret, clientSecret, StringComparison.CurrentCultureIgnoreCase);

                if (!isClientIdSame || !isClientSecretSame)
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("ClientAttribute Exception {ex}", ex);
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}