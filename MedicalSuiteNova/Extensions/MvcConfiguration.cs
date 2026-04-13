using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Extensions
{
    public static class MvcConfiguration
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorMessages = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => {
                                if (e.ErrorMessage.Contains("JSON deserialization"))
                                    return "El formato del JSON es inválido o faltan campos obligatorios.";

                                return e.ErrorMessage;
                            })
                            .ToList();

                        var customResponse = new
                        {
                            status = 400,
                            errors = errorMessages
                        };

                        return new BadRequestObjectResult(customResponse);
                    };
                });

            return services;
        }
    }
}
