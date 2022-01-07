namespace Cashflow.Api.Contracts
{
    public interface IAppConfig
    {
        string DatabaseConnectionString { get; }

        int CookieExpiresInMinutes { get; }

        string SecretJwtKey { get; }

        bool IsDevelopment { get; }
    }
}