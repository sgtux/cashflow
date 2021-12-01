using System.Text.RegularExpressions;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(u => u.NickName).MinimumLength(4).WithMessage(ValidatorMessages.FieldMinLength("Nick Name", 4));
            RuleFor(u => u.Password).MinimumLength(8).WithMessage(ValidatorMessages.FieldMinLength("Senha", 8));
            RuleFor(u => u).Must(ValidNickNameInUse).WithMessage(ValidatorMessages.User.NickNameAlreadyInUse);
            RuleFor(u => u).Must(ValidNickNamePattern).WithMessage(ValidatorMessages.User.NickNamePattern);
        }

        private bool ValidNickNamePattern(User user)
        {
            return Regex.IsMatch(user.NickName, "^[a-zA-Z0-9_$#@!&]{1,}$");
        }

        private bool ValidNickNameInUse(User user)
        {
            var resultDb = _userRepository.FindByNickName(user.NickName).Result;
            return resultDb == null || resultDb.Id == user.Id;
        }
    }
}