namespace Cashflow.Api.Shared
{
  /// App configuration
  public class AppConfiguration
  {
    private readonly string _jwtKey;

    /// Constructor
    public AppConfiguration(string jwtKey)
    {
      _jwtKey = jwtKey;
    }

    /// Jwt key
    public string JwtKey => _jwtKey;
  }
}