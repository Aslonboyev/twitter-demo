using BlogApp.WebApi.Exceptions;
using System.Text.Json;

namespace BlogApp.WebApi.Middlewares
{
    public class ExceptionHadlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHadlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (StatusCodeException exeption)
            {
                await HandleAsync(exeption, httpContext);
            }
            catch (Exception error)
            {
                await HandleOtherExceptionAsync(error, httpContext);
            }
        }

        private async Task HandleOtherExceptionAsync(Exception error, HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            { message = error.Message }));
        }

        public async Task HandleAsync(StatusCodeException statusCodeException, HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)statusCodeException.HttpStatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            { message = statusCodeException.Message }));
        }
    }
}
