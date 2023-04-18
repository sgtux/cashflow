using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cashflow.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Cashflow.Api.Shared;
using Cashflow.Api.Contracts;
using Cashflow.Api;
using Cashflow.Tests.Mocks;

namespace Cashflow.Tests.Config
{
    public class TestStartup
    {
        public TestStartup(IHostEnvironment env) { }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = new TestAppConfig();

            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly).AddControllersAsServices();
            services.ConfigureAuthentication(appConfig.SecretJwtKey);
            services.AddRouting();
            services.AddSingleton<IAppConfig>(appConfig);
            services.ConfigureServices();
            services.ConfigureCaches();
            services.ConfigureRepositories();
            services.AddScoped<IDatabaseContext, TestDatabaseContext>();
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