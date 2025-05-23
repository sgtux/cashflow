using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Services;
using Cashflow.Api.Shared;
using Cashflow.Api.Shared.Cache;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<FuelExpenseService>();
            services.AddScoped<RemainingBalanceService>();
            services.AddScoped<RecurringExpenseService>();
            services.AddScoped<ProjectionService>();
            services.AddScoped<LogService>();            
        }

        public static void ConfigureCaches(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<AppCache>();
            services.AddScoped<HomeCache>();
            services.AddScoped<ProjectionCache>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<IEarningRepository, EarningRepository>();
            services.AddScoped<IHouseholdExpenseRepository, HouseholdExpenseRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IFuelExpenseRepository, FuelExpenseRepository>();
            services.AddScoped<IRemainingBalanceRepository, RemainingBalanceRepository>();
            services.AddScoped<IRecurringExpenseRepository, RecurringExpenseRepository>();
            services.AddScoped<ISystemParameterRepository, SystemParameterRepository>();
        }

        public static void ConfigureDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, DatabaseContext>();
        }
    }
}