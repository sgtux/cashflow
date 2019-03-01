using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApi.Infra;
using FinanceApi.Infra.Repository;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Cashflow.Api
{
#pragma warning disable CS1591
  public class Startup
  {
    private IHostingEnvironment _env;

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddEnvironmentVariables();

      if (!env.IsStaging())
        builder = builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

      Configuration = builder.Build();
      _env = env;
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      string connectionString = null;
      string jwtKey = null;
      if (_env.IsDevelopment())
      {
        connectionString = Configuration.GetConnectionString("FinanceDB");
        jwtKey = Configuration["FinanceJwtKey"];
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
      }
      else if (_env.IsProduction())
      {
        connectionString = Environment.GetEnvironmentVariable("FinanceDB");
        jwtKey = Configuration["FinanceJwtKey"];
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
      }
      else
      {
        jwtKey = "!@#$%&*()SecretKey!TTESTT!@#$%&*()";
        services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase(databaseName: "cashflow_db"));
      }

      services.AddMvc();
      CofigureScopes(services);

      ConfigureAuth(services, jwtKey);

      services.AddSingleton(typeof(AppConfiguration), new AppConfiguration(jwtKey));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Cashflow API", Version = "v1" });
        var filePath = Path.Combine(System.AppContext.BaseDirectory, "Api.xml");
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

    public void Configure(IApplicationBuilder app)
    {
      app.UseAuthentication();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cashflow API V1");
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

    private void ConfigureAuth(IServiceCollection services, string jwtKey)
    {
      var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secretKey
      };

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(
        options =>
        {
          options.TokenValidationParameters = tokenValidationParameters;
        }
      );
    }
  }
}
