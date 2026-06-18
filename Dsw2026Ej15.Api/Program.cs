using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores tradicionales
builder.Services.AddControllers();

// Punto 3.f: Registrar la persistencia en memoria como SINGLETON
builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

// Punto 3.j: Registrar el servicio base para el sondeo de estado
builder.Services.AddHealthChecks();

var app = builder.Build();

// Punto 3.i: Activar el Middleware de excepciones globales (DEBE ir arriba de todo)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

// Punto 3.j: Mapear el sondeo de estado básico en la ruta /health-check
app.MapHealthChecks("/health-check");

app.MapControllers();

app.Run();