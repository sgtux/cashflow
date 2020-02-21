using Cashflow.Api.Shared;

namespace Cashflow.Api.Service
{
  public class BaseService
  {
    protected void ThrowValidationError(string error) => throw new ValidateException(error);
  }
}