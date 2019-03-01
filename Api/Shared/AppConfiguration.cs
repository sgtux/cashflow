namespace FinanceApi.Shared
{
  public class AppConfiguration
  {
    private readonly string _jwtKey;

    public AppConfiguration(string jwtKey)
    {
      _jwtKey = jwtKey;
    }

    public string JwtKey => _jwtKey;
  }
}