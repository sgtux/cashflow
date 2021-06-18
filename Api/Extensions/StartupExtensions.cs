using System.Text;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureScopes(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<CreditCardService>();
            services.AddScoped<SalaryService>();
            services.AddScoped<LogService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();

            services.AddScoped<DatabaseContext>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, string jwtKey)
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
            .AddJwtBearer(options => options.TokenValidationParameters = tokenValidationParameters);
        }
    }
}