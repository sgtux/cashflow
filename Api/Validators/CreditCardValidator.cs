using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class CreditCardValidator : AbstractValidator<CreditCard>
    {
        public CreditCardValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.UserId).GreaterThan(0);
        }
    }
}