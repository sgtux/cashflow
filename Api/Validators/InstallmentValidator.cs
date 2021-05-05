using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class InstallmentValidator : AbstractValidator<Installment>
    {
        public InstallmentValidator()
        {
            RuleFor(p => p.Cost).GreaterThan(0);
            RuleFor(p => p.Date).NotNull().NotEqual(default(System.DateTime)).WithMessage("'Date' has an 'Invalid date'");
            RuleFor(p => p.Number).GreaterThan(0);
        }
    }
}