using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EsameFinale.Middlewares
{
    public class ExceptionsMiddleware : IMiddleware
    {
        private readonly ILogger<Exception> _logger;

        public ExceptionsMiddleware(ILogger<Exception> logger){
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try {
                await next(context);
            } catch (InvalidOperationException ex){
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 422;
                await context.Response.WriteAsJsonAsync(ex.Message);
            } 
        }
    }
}