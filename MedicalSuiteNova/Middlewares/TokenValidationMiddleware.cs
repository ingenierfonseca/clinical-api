namespace MedicalSuiteNova.Api.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Intentar obtener el token del Header
            var token = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                // Opcional: Podrías dejarlo pasar y que el atributo [Authorize] se encargue
                await _next(context);
                return;
            }

            // 2. Ejemplo de lógica personalizada: 
            // ¿El token está bloqueado?
            if (token == "token_robado_ejemplo")
            {
                throw new UnauthorizedAccessException("Este token ha sido revocado.");
            }

            await _next(context);
        }
    }
}
