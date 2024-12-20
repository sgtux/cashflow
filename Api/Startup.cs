using Cashflow.Api.Auth;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddControllers();
            services.AddRouting();
            services.AddSingleton<IAppConfig>(appConfig);
            services.ConfigureServices();
            services.ConfigureCaches();
            services.ConfigureRepositories();
            services.ConfigureDatabaseContext();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ExceptionHandler));

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseRouting();

            app.UseMiddleware(typeof(AuthenticationMiddleware));
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}