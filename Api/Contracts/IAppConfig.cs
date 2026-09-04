namespace Cashflow.Api.Contracts
{
    public interface IAppConfig
    {
        string DatabaseConnectionString { get; }

        int CookieExpiresInMinutes { get; }

        string SecretJwtKey { get; }

        string DataEncryptionKey { get; }

        bool IsDevelopment { get; }

        bool IsTest { get; }

        string GoogleOauthAccessTokenUrl { get; }

        string GoogleOauthIdTokenUrl { get; }

        string GoogleClientId { get; }
    }
}