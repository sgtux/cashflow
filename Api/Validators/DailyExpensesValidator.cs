using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class DailyExpensesValidator : AbstractValidator<DailyExpenses>
    {
        private IDailyExpensesRepository _repository;

        public DailyExpensesValidator(IDailyExpensesRepository repository)
        {
            _repository = repository;
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.DailyExpenses.InvalidDate);
            RuleFor(s => s.ShopName).NotEmpty().WithMessage(ValidatorMessages.DailyExpenses.InvalidShopName);
            RuleFor(s => s.Items).Custom(ValidateItems);
        }

        private void ValidateItems(IList<DailyExpensesItem> list, ValidationContext<DailyExpenses> context)
        {
            if (list == null || list.Count == 0)
            {
                context.AddFailure(ValidatorMessages.DailyExpenses.InvalidItemsCount);
                return;
            }

            if (list.Any(p => p.Price <= 0))
                context.AddFailure(ValidatorMessages.DailyExpenses.ItemsWithInvalidPrice);

            if (list.Any(p => string.IsNullOrWhiteSpace(p.ItemName)))
                context.AddFailure(ValidatorMessages.DailyExpenses.ItemsWithInvalidName);

            if (list.Any(p => p.Amount <= 0 || p.Amount > 1000))
                context.AddFailure(ValidatorMessages.DailyExpenses.ItemsWithInvalidAmount);
        }
    }
}