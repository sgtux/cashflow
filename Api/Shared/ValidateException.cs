namespace FinanceApi.Shared
{
  /// Validate exception
  public class ValidateException : System.Exception
  {
    /// Constructor
    public ValidateException(string message) : base(message) { }
  }
}