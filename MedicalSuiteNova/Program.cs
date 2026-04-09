using MedicalSuiteNova.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();

// Permitir conexion react local
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReact", policy => {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://wonderful-glacier-0c92d611e.6.azurestaticapps.net"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Permitir conexion react local
app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();
