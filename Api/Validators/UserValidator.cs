using System.Text.RegularExpressions;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(u => u.Email).MinimumLength(4).WithMessage(ValidatorMessages.FieldMinLength("Email", 6));
            RuleFor(u => u.Password).MinimumLength(8).WithMessage(ValidatorMessages.FieldMinLength("Senha", 8));
            RuleFor(u => u).Must(ValidEmailInUse).WithMessage(ValidatorMessages.User.EmailAlreadyInUse);
            RuleFor(u => u).Must(ValidEmailPattern).WithMessage(ValidatorMessages.User.EmailPattern);
        }

        private bool ValidEmailPattern(User user) => Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private bool ValidEmailInUse(User user)
        {
            var resultDb = _userRepository.FindByEmail(user.Email).Result;
            return resultDb == null || resultDb.Id == user.Id;
        }
    }
}