using Cashflow.Api.Extensions;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.ConfigureAuthentication(appConfig.SecretJwtKey);
            services.AddRouting();
            services.AddSingleton(appConfig);
            services.ConfigureScopes();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ExceptionHandler));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}