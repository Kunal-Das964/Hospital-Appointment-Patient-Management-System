using HospitalManagement.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace HospitalManagement.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
            catch (SqlException ex) when (ex.Number == 50001 || ex.Number == 50002)
            {
                context.Response.StatusCode = 409;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
            catch (SqlException ex) when (ex.Number == 50003 || ex.Number == 50004 || ex.Number == 50005)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    //error = "An unexpected error occurred. Please try again later."
                    error = ex.Message,        // Temporarily real error dikhao
                    details = ex.StackTrace    // Stack trace bhi dikhao
                });
            }
        }
    }
}
