using Microsoft.AspNetCore.Http.Extensions;

namespace NewZealandWalk.API.Middlewares
{
    public class ResponseTrackerMiddleware
    {
        private readonly ILogger<ResponseTrackerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ResponseTrackerMiddleware(ILogger<ResponseTrackerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode != 200)
            {
                string url = httpContext.Request.GetDisplayUrl();
                string body = string.Empty;
                if (httpContext.Request.Body != null)
                {
                    body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                }

                if (httpContext.Response.StatusCode == 204)
                {
                    _logger.LogInformation($"204:{url}:{body}");
                }
                else if (httpContext.Response.StatusCode == 400)
                {
                    _logger.LogWarning($"400:{url}:{body}");
                }
                else if (httpContext.Response.StatusCode == 500)
                {
                    _logger.LogCritical($"500:{url}:{body}");
                }
                else
                {
                    _logger.LogError($"{httpContext.Response.StatusCode}:{url}:{body}");
                }
            }

            await _next(httpContext);
        }
    }
}