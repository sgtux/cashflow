using System.Linq;
using Cashflow.Api.Infra.Entity;
using FluentValidation.Validators;

namespace Cashflow.Api.Validators
{
    public class InstallmentPropertyValidator : PropertyValidator
    {
        public InstallmentPropertyValidator() : base() { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            // var payment = context.Instance as Payment;
            // if (payment?.Installments != null)
            // {
            //     foreach (var i in payment.Installments)
            //     {
            //         var validatorResult = new InstallmentValidator().Validate(i);
            //         if (!validatorResult.IsValid)
            //         {
            //             var errors = string.Join(',', validatorResult.Errors.Select(p => p.ErrorMessage));
            //             context.MessageFormatter.AppendArgument("Error", errors);
            //             return false;
            //         }
            //     }
            // }
            return true;
        }
    }
}