using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactsAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;

        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, ex.Message);

            // Determine the response status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create the error response
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred.",
                Details = ex.Message // Consider removing this in production
            };

            context.Response.ContentType = "application/json";
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalExceptionExtensions
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalException>();
        }
    }
}
