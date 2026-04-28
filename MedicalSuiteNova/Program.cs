using MedicalSuiteNova.Api.Extensions;
using MedicalSuiteNova.Api.Middlewares;
using MedicalSuiteNova.Api.Services;
using MedicalSuiteNova.Application;
using MedicalSuiteNova.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Implementar Token
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCustomControllers();
builder.Services.AddCustomCors(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Middlewares
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseStaticFiles();//para poder mostrar las imagenes

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
