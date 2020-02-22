namespace Cashflow.Api.Shared
{
  public class AppConfig
  {
    public string JwtKey { get; private set; }
    public string ConnectionString { get; private set; }

    public AppConfig(string jwtKey, string connectionString)
    {
      JwtKey = jwtKey;
      ConnectionString = connectionString;
    }
  }
}