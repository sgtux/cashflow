using Cashflow.Api.Contracts;

namespace Cashflow.Tests.Config
{
    public class TestAppConfig : IAppConfig
    {
        private readonly string _environmentName;

        public TestAppConfig()
        {
            _environmentName = "Development";
            DatabaseConnectionString = "Data Source=test.db";
            CookieExpiresInMinutes = 1440;
            SecretJwtKey = "!@#$TestSecretKey!WebApi!@#$%&*()";
        }

        public string DatabaseConnectionString { get; }

        public int CookieExpiresInMinutes { get; }

        public string SecretJwtKey { get; }

        public bool IsDevelopment => _environmentName == "Development";
    }
}