using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinanceApi.Infra;
using FinanceApi.Infra.Repository;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace FinanceApi
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      string connectionString = Configuration.GetConnectionString("Finance");
      string jwtKey = Configuration["JwtKey"];
      services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
      services.AddMvc();
      CofigureScopes(services);
      services.AddSingleton(typeof(AppConfiguration), new AppConfiguration(jwtKey));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Finance API", Version = "v1" });
        var filePath = Path.Combine(System.AppContext.BaseDirectory, "FinanceApi.xml");
        c.IncludeXmlComments(filePath);
        c.AddSecurityDefinition("Token", new ApiKeyScheme
        {
          Description = "Adicione o token.",
          Name = "token",
          In = "header",
          Type = "apiKey"
        });
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance API V1");
      });

      app.UseMiddleware(typeof(ExceptionHandler));
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseMvc();
    }

    private void CofigureScopes(IServiceCollection services)
    {
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPaymentRepository, PaymentRepository>();
      services.AddScoped<ICreditCardRepository, CreditCardRepository>();
    }
  }
}
