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

            MicrosoftClientId = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID");
            MicrosoftTenantId = Environment.GetEnvironmentVariable("MICROSOFT_TENANT_ID");
            MicrosoftRedirectUri = Environment.GetEnvironmentVariable("MICROSOFT_REDIRECT_URI");

            AuthApiUrl = Environment.GetEnvironmentVariable("AUTH_API_URL");
        }

        public string DatabaseConnectionString { get; }

        public int CookieExpiresInMinutes { get; }

        public string SecretJwtKey { get; }

        public string DataEncryptionKey { get; }

        public bool IsDevelopment => _environmentName == "Development";

        public bool IsTest => false;

        public string GoogleOauthAccessTokenUrl { get; }

        public string GoogleOauthIdTokenUrl { get; }

        public string GoogleClientId { get; }

        public string MicrosoftClientId { get; }

        public string MicrosoftTenantId { get; }

        public string MicrosoftRedirectUri { get; }

        public string AuthApiUrl { get; }
    }
}