using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
  public class UserValidator : AbstractValidator<User>
  {
    public UserValidator()
    {
      RuleFor(user => user.Name).NotNull().NotEmpty().MinimumLength(4);
      RuleFor(user => user.Email).EmailAddress();
      RuleFor(user => user.Password).NotNull().MinimumLength(6).When(user => user.Id == 0);
    }
  }
}