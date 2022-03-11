using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class CreditCardValidator : AbstractValidator<CreditCard>
    {
        private ICreditCardRepository _creditCardRepository;

        private IUserRepository _userRepository;

        public CreditCardValidator(ICreditCardRepository creditCardRepository, IUserRepository userRepository)
        {
            _creditCardRepository = creditCardRepository;
            _userRepository = userRepository;
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Nome"));
            RuleFor(c => c.InvoiceClosingDay).InclusiveBetween(1, 30).WithMessage(ValidatorMessages.BetweenValue("Dia de fechamento da fatura", 1, 30));
            RuleFor(c => c.InvoiceDueDay).InclusiveBetween(1, 30).WithMessage(ValidatorMessages.BetweenValue("Dia de vencimento da fatura", 1, 30));
            RuleFor(c => c).Must(CreditCardExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
            RuleFor(c => c).Must(UserExists).WithMessage(ValidatorMessages.NotFound("Usuário"));
        }

        public bool CreditCardExists(CreditCard card)
        {
            var cards = _creditCardRepository.GetSome(new BaseFilter() { UserId = card.UserId }).Result;
            return cards.Any(p => p.Id == card.Id);
        }

        public bool UserExists(CreditCard card)
        {
            return card.UserId > 0 && _userRepository.GetById(card.UserId).Result != null;
        }
    }
}