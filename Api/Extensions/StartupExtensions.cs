using System.Text;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Services;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<HomeService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<CreditCardService>();
            services.AddScoped<EarningService>();
            services.AddScoped<HouseholdExpenseService>();
            services.AddScoped<VehicleService>();
            services.AddScoped<FuelExpensesService>();
            services.AddScoped<RemainingBalanceService>();
            services.AddScoped<RecurringExpenseService>();
            services.AddScoped<ProjectionService>();
            services.AddScoped<LogService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<IEarningRepository, EarningRepository>();
            services.AddScoped<IHouseholdExpenseRepository, HouseholdExpenseRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IFuelExpensesRepository, FuelExpensesRepository>();
            services.AddScoped<IRemainingBalanceRepository, RemainingBalanceRepository>();
            services.AddScoped<IRecurringExpenseRepository, RecurringExpenseRepository>();
        }

        public static void ConfigureDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, DatabaseContext>();
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