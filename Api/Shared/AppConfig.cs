using System;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Shared
{
    public class AppConfig : IAppConfig
    {
        private readonly string _environmentName;

        public AppConfig()
        {
            _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            DatabaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            CookieExpiresInMinutes = Convert.ToInt32(Environment.GetEnvironmentVariable("COOKIE_EXPIRES_IN_MINUTES"));
            SecretJwtKey = Environment.GetEnvironmentVariable("SECRET_JWT_KEY");
        }

        public string DatabaseConnectionString { get; }

        public int CookieExpiresInMinutes { get; }

        public string SecretJwtKey { get; }

        public bool IsDevelopment => _environmentName == "Development";
    }
}