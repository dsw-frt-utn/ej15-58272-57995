using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Dsw2026Ej15.Domain;
using Microsoft.AspNetCore.Http;

namespace Dsw2026Ej15.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continúa el flujo normal
            }
            catch (ValidationException ex)
            {
                // Punto 3.i.i: Si es ValidationException, devuelve 400 Bad Request
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                // Punto 3.i.ii: Cualquier otro caso, devuelve 500
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal Server Error.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}