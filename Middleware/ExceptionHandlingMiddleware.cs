using System.Net;
using System.Text.Json;

namespace ApexWebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                statusCode = (int)statusCode,
                message,
                detail = _env.IsDevelopment() ? exception.ToString() : null
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
