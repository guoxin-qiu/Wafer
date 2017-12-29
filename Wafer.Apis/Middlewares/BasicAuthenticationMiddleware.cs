using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Wafer.Apis.Middlewares
{
    public sealed class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authentication = context.Request.Headers["Authorization"];
            
            if(!string.IsNullOrWhiteSpace(authentication))
            {
                //extract credentials
                //balabala

                if (true) // check authentication
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
