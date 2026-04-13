using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace MedicalSuiteNova.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Logueamos el error para nosotros (opcional pero muy útil)
            _logger.LogError(exception, "Ha ocurrido un error no manejado: {Message}", exception.Message);

            // 2. Determinamos el código de estado según el tipo de excepción
            var statusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            // 3. Creamos el objeto de respuesta con el formato que te gusta
            var response = new
            {
                status = statusCode,
                errors = new List<string> { exception.Message }
            };

            // 4. Configuramos la respuesta HTTP
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            // 5. Escribimos el JSON en el cuerpo de la respuesta
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            // Retornamos true para indicar que ya manejamos la excepción
            return true;
        }
    }
}
