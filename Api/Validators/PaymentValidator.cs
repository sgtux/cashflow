using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        public PaymentValidator(IPaymentRepository paymentRepository, ICreditCardRepository creditCardRepository)
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(p => p.Description).NotEmpty().WithMessage(ValidatorMessages.Payment.DescriptionRequired);
            RuleFor(p => p.Installments).NotEmpty().WithMessage(ValidatorMessages.Payment.InstallmentsRequired);
            RuleFor(p => p.Type).IsInEnum().WithMessage(ValidatorMessages.Payment.PaymentTypeInvalid);
            RuleFor(p => p).Must(ValidFixedPayment).When(p => p.FixedPayment).WithMessage(ValidatorMessages.Payment.FixedPaymentWithMoreThenOnePlot);
            RuleFor(p => p).Must(ValidCreditCard).WithMessage(ValidatorMessages.CreditCard.NotFound);
            RuleFor(p => p).Must(ValidPayment).When(p => p.Id > 0).WithMessage(ValidatorMessages.Payment.NotFound);
            RuleFor(p => p.Installments).Custom(ValidateInstallments);
        }

        private void ValidateInstallments(IList<Installment> list, ValidationContext<Payment> context)
        {
            if (list != null)
            {
                if (list.Any(p => p.Cost <= 0))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidValue);

                if (list.Any(p => p.Date == default(DateTime)))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidDate);

                if (list.Any(p => p.PaidDate == default(DateTime)))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidPaidDate);

                if (list.Any(p => p.Number <= 0 || p.Number > 72))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidNumber);

                if (list.Select(p => p.Number).Distinct().Count() != list.Count)
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithRepeatednNumbers);

                if (list.Count > 72)
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithMaxLengthExceded);
            }
        }

        private bool ValidFixedPayment(Payment payment)
        {
            return payment.Installments.Count() == 1;
        }

        private bool ValidCreditCard(Payment payment)
        {
            if (payment.CreditCardId > 0)
            {
                var card = _creditCardRepository.GetByUserId(payment.UserId).Result.FirstOrDefault(p => p.Id == payment.CreditCardId.Value);
                if (card == null || card.UserId != payment.UserId)
                    return false;
            }
            else
                payment.CreditCardId = null;
            return true;
        }

        private bool ValidPayment(Payment payment)
        {
            var paymentDb = _paymentRepository.GetById(payment.Id).Result;
            return paymentDb?.UserId == payment.UserId;
        }
    }
}