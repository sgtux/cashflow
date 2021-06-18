using System.Linq;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
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
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidatorMessages.CreditCard.NameRequired);
            RuleFor(c => c.UserId).GreaterThan(0).WithMessage(ValidatorMessages.CreditCard.UserIdRequired);
            RuleFor(c => c).Must(CreditCardExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.CreditCard.NotFound);
            RuleFor(c => c).Must(UserExists).WithMessage(ValidatorMessages.User.NotFound);
        }

        public bool CreditCardExists(CreditCard card)
        {
            var cards = _creditCardRepository.GetByUserId(card.UserId).Result;
            return cards.Any(p => p.Id == card.Id);
        }

        public bool UserExists(CreditCard card)
        {
            return _userRepository.GetById(card.UserId).Result != null;
        }
    }
}