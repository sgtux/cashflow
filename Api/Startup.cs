using Cashflow.Api.Extensions;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cashflow.Api
{
    public class Startup
    {
        private bool _isDevelopment;
        public Startup(IHostEnvironment env) { }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = new AppConfig();
            _isDevelopment = appConfig.IsDevelopment;

            services.AddControllers();
            services.ConfigureScopes();
            services.ConfigureAuthentication(appConfig.SecretJwtKey);
            services.AddRouting();
            services.AddSingleton(appConfig);
            services.ConfigureScopes();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cashflow API", Version = "v1" });
                var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                var filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, $"{appName}.xml");
                c.IncludeXmlComments(filePath);
                c.AddSecurityDefinition("Token", new OpenApiSecurityScheme
                {
                    Name = "Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ExceptionHandler));
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            if (_isDevelopment)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cashflow API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}