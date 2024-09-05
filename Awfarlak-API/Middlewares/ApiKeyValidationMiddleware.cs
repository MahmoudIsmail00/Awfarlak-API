namespace Awfarlak_API.Middlewares
{
    public class ApiKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-API-KEY";
        private const string ApiKey = "123";

        public ApiKeyValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) || extractedApiKey != ApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized request. API Key is missing or invalid.");
                return;
            }

            await _next(context);
        }
    }
}
