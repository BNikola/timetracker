using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimeTrackerApp
{
    public class ErrorHandlingMiddleware
    {
        private RequestDelegate _next;
        private ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            // TODO: change code based on exception

            var problem = new ProblemDetails
            {
                Type = "http://www.etf.unibl.org/server-error",
                Title = "Internal server error",
                Detail = ex.Message,
                Instance = "",
                Status = (int)code
            };

            // serijalizovati u json - System.Text.Json.Serialization
            var result = JsonSerializer.ToString(problem);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
