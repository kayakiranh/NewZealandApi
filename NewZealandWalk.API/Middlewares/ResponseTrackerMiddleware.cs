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
            if (httpContext.Response.StatusCode != 204)
            {
                string controllerName = httpContext.Request.RouteValues["controller"].ToString();
                string actionName = httpContext.Request.RouteValues["action"].ToString();

                string absoluteUri = string.Concat(
                    httpContext.Request.Scheme,
                    "://",
                    httpContext.Request.Host.ToUriComponent(),
                    httpContext.Request.PathBase.ToUriComponent(),
                    httpContext.Request.Path.ToUriComponent(),
                    httpContext.Request.QueryString.ToUriComponent()
                );

                if (httpContext.Response.StatusCode == 204)
                {
                    _logger.LogInformation($"204:{controllerName}:{actionName}, {absoluteUri}");
                }
                else if (httpContext.Response.StatusCode == 400)
                {
                    _logger.LogWarning($"400:{controllerName}:{actionName}, {absoluteUri}");
                }
                else if (httpContext.Response.StatusCode == 500)
                {
                    _logger.LogCritical($"500:{controllerName}:{actionName}, {absoluteUri}");
                }
                else
                {
                    _logger.LogError($"{httpContext.Response.StatusCode}:{controllerName}:{actionName}, {absoluteUri}");
                }
            }

            await _next(httpContext);
        }
    }
}