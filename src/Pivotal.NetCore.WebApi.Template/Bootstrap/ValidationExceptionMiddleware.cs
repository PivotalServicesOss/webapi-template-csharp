using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pivotal.NetCore.WebApi.Template.Bootstrap
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException validationException)
            {
                var response = context.Response;
                
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status400BadRequest;
                await response.WriteAsync(JsonConvert.SerializeObject(new 
                {
                    Message = validationException.Message,
                    Description = validationException.StackTrace
                }));
            }
        }

    }
}