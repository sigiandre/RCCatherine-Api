using System.Net;
using System.Text.Json;

namespace RCCatherineApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la solicitud {Method} {Path}", context.Request.Method, context.Request.Path);
                await ManejarExcepcionAsync(context, ex);
            }
        }

        private static Task ManejarExcepcionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, mensaje) = ex switch
            {
                ArgumentException or InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor.")
            };

            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                mensaje
            });

            return context.Response.WriteAsync(payload);
        }
    }
}