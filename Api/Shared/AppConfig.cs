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
            DataEncryptionKey = Environment.GetEnvironmentVariable("DATA_ENCRYPTION_KEY");
            GoogleOauthAccessTokenUrl = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_ACCESS_TOKEN");
            GoogleOauthIdTokenUrl = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_ID_TOKEN");
            GoogleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        }

        public string DatabaseConnectionString { get; }

        public int CookieExpiresInMinutes { get; }

        public string SecretJwtKey { get; }

        public string DataEncryptionKey { get; }

        public bool IsDevelopment => _environmentName == "Development";

        public string GoogleOauthAccessTokenUrl { get; }

        public string GoogleOauthIdTokenUrl { get; }

        public string GoogleClientId { get; }
    }
}