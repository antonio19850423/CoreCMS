using System.Net;
using System.Text.Json;

namespace Velora.Identity.Middlewares
{
    /// <summary>
    /// Middleware to handle global exceptions and return a standardized JSON error response.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continue processing the request
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Build a standard error response
                var response = new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred.",
                    Details = ex.Message, // In production, you might omit this for security
                    StackTrace = ex.StackTrace // Optional: remove in production if needed
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = response.StatusCode;

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }

    /// <summary>
    /// Extension method to easily add the middleware to the HTTP pipeline.
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
