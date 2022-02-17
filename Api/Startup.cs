using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cashflow.Api
{
    public class Startup
    {
        public Startup(IHostEnvironment env) { }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = new AppConfig();
            services.AddControllers();
            services.ConfigureAuthentication(appConfig.SecretJwtKey);
            services.AddRouting();
            services.AddSingleton<IAppConfig>(appConfig);
            services.ConfigureServices();
            services.ConfigureRepositories();
            services.ConfigureDatabaseContext();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ExceptionHandler));

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}