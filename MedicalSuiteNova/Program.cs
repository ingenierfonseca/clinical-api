using MedicalSuiteNova.Api.Extensions;
using MedicalSuiteNova.Api.Middlewares;
using MedicalSuiteNova.Api.Services;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application;
using MedicalSuiteNova.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Threading.RateLimiting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration)
              .Enrich.FromLogContext();
    });

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    // Implementar Token
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddJwtAuthentication(builder.Configuration);

    // Middleware para validar permisos en api
    builder.Services.AddCustomAuthorization();

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddCustomControllers();
    builder.Services.AddCustomCors(builder.Configuration);

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddFixedWindowLimiter("Global", opt =>
        {
            opt.PermitLimit = 5;
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 0;
        });
    });

    var app = builder.Build();

    // Middlewares
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseCors("AllowReact");
    app.UseStaticFiles();//para poder mostrar las imagenes

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación terminó inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
