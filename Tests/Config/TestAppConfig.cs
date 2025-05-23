using Cashflow.Api.Contracts;

namespace Cashflow.Tests.Config
{
    public class TestAppConfig : IAppConfig
    {
        public string DatabaseConnectionString => "Data Source=test.db";

        public int CookieExpiresInMinutes => 1440;

        public string SecretJwtKey => "!@#$TestSecretKey!WebApi!@#$%&*()";

        public bool IsDevelopment => false;

        public string GoogleOauthAccessTokenUrl => throw new System.NotImplementedException();

        public string GoogleOauthIdTokenUrl => throw new System.NotImplementedException();

        public string DataEncryptionKey => "1234567890123456";

        public string GoogleClientId => "123.apps.googleusercontent.com";
    }
}