using Microsoft.AspNetCore.Builder;

namespace Wafer.Apis.Middlewares
{
    public static class BasicAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
