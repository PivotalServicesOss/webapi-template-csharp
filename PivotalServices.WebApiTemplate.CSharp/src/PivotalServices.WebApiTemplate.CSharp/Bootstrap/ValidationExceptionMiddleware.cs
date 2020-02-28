using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PivotalServices.WebApiTemplate.CSharp.Bootstrap
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ValidationExceptionMiddleware> logger;

        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ValidationException validationException)
            {
                logger.LogError($"Message: {validationException.Message}, Details:{validationException.StackTrace}");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                {
                    validationException.Message,
                }));
            }
        }

    }
}