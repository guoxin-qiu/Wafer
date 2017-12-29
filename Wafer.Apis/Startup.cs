using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wafer.Apis.Middlewares;
using Wafer.Apis.Models;
using Wafer.Apis.Utils;

namespace Wafer.Apis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WaferContext>(options => options.UseInMemoryDatabase("wafer-memory-db"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins(Configuration["Settings:Origins"]?.Split(";"))
                    .AllowCredentials().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddAuthentication(StaticString.ApiCookieAuthenticationSchema)
                .AddCookie(StaticString.ApiCookieAuthenticationSchema, options => {
                    options.AccessDeniedPath = "/Account/Forbidden";
                    options.LoginPath = "/Account/Unauthorized";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = System.TimeSpan.FromDays(1);
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WaferContext waferContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigin");

            // Call UseAuthentication before calling UseMVCWithDefaultRoute or UseMVC.
            app.UseAuthentication();

            app.UseMvc();

            WaferContextInitializer.Init(waferContext);
        }
    }
}
