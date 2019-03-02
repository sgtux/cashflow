using Cashflow.Api.Shared;

namespace Cashflow.Api.Service
{
  /// Base service
  public class BaseService
  {
    /// Create a new validation exception
    protected void ThrowValidationError(string error) => throw new ValidateException(error);
  }
}