using System.Linq;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
  public class PaymentValidator : AbstractValidator<Payment>
  {
    public PaymentValidator()
    {
      RuleFor(p => p.Description).NotEmpty();
      RuleFor(p => p.Installments).NotEmpty();
      RuleFor(p => p.Type).IsInEnum();
      RuleFor(p => p.FixedPayment && p.Installments.Count() > 1).NotEqual(true)
        .WithMessage("Fixed payments cannot have more than one installment");
      RuleFor(p => p.Installments).SetValidator(new InstallmentPropertyValidator());
    }
  }
}