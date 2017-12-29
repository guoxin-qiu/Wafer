using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Wafer.Apis.Middlewares;
using Wafer.Apis.Models;

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

            services.AddMemoryCache();

            services.AddMvc();

            services.AddSwaggerGen(sg => {
                sg.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Wafer Apis Document",
                    Description = "RESTful Apis for Wafer",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Denis", Url = "https://github.com/guoxin-qiu/Wafer" }
                });

                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "Wafer.Apis.xml");
                //sg.IncludeXmlComments(xmlPath);

                sg.OperationFilter<HttpHeaderOperation>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WaferContext waferContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigin");
            
            app.UseBasicAuthenticationMiddleware();

            app.UseSwagger();

            app.UseSwaggerUI(sg =>
            {
                sg.SwaggerEndpoint("/swagger/v1/swagger.json", "Wafer Apis V1");
                sg.ShowRequestHeaders();
            });

            app.UseMvc();

            WaferContextInitializer.Init(waferContext);
        }
    }
}
