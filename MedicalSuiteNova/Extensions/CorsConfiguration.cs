namespace MedicalSuiteNova.Api.Extensions
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173", // Desarrollo local (Vite)
                        "https://wonderful-glacier-0c92d611e.6.azurestaticapps.net" // Azure Production
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
