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
            RuleFor(p => p.Description).NotEmpty().WithMessage("A descrição deve ser preenchida");
            RuleFor(p => p.Installments).NotEmpty().WithMessage("O pagamento deve ter pelo menos 1 parcela.");
            RuleFor(p => p.Type).IsInEnum().WithMessage("O tipo do pagamento é inválido.");
            RuleFor(p => p.FixedPayment && p.Installments.Count() > 1).NotEqual(true)
              .WithMessage("Pagamento fixo não pode ter mais de uma parcela.");
            RuleFor(p => p.Installments).SetValidator(new InstallmentPropertyValidator());
            RuleFor(p => ValidateCreditCard(p)).NotEqual(false).WithMessage("Cartão de crédito não encontrado.");
        }

        private bool ValidateCreditCard(Payment payment)
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
    }
}