using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Wafer.Apis.Middlewares
{
    public sealed class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        public BasicAuthenticationMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // TODO: How to implement pages limit?

            if (context.Request.Path.Value.ToLower().Contains("account/login"))
            {
                await _next.Invoke(context);
                return;
            }

            string authentication = context.Request.Headers["Authorization"];
            
            if(!string.IsNullOrWhiteSpace(authentication))
            {
                var token = string.Empty;
                if (_memoryCache.TryGetValue(authentication,out token))
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
