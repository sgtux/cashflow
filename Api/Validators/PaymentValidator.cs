using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
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
            RuleFor(p => p.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(p => p.Installments).NotEmpty().WithMessage(ValidatorMessages.Payment.InstallmentsRequired);
            RuleFor(p => p.TypeId).IsInEnum().WithMessage(ValidatorMessages.Payment.PaymentTypeInvalid);
            RuleFor(p => p).Must(ValidCreditCard).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
            RuleFor(p => p).Must(ValidPayment).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Pagamento"));
            RuleFor(p => p.Installments).Custom(ValidateInstallments);
        }

        private void ValidateInstallments(IList<Installment> list, ValidationContext<Payment> context)
        {
            if (list != null)
            {
                if (list.Any(p => p.Value <= 0))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidValue);

                if (list.Any(p => p.Date == default(DateTime)))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidDate);

                if (list.Any(p => p.PaidDate == default(DateTime)))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidPaidDate);

                if (list.Any(p => p.Number <= 0 || p.Number > 72))
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithInvalidNumber);

                if (list.Select(p => p.Number).Distinct().Count() != list.Count)
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithRepeatedNumbers);

                if (list.Count > 72)
                    context.AddFailure(ValidatorMessages.Payment.InstallmentWithMaxLengthExceded);
            }
        }

        private bool ValidCreditCard(Payment payment)
        {
            if (payment.CreditCardId > 0)
            {
                var card = _creditCardRepository.GetSome(new BaseFilter() { UserId = payment.UserId }).Result.FirstOrDefault(p => p.Id == payment.CreditCardId.Value);
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