using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class RecurringExpenseValidator : AbstractValidator<RecurringExpenseEntity>
    {
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        public RecurringExpenseValidator(IRecurringExpenseRepository recurringExpenseRepository, ICreditCardRepository creditCardRepository)
        {
            _recurringExpenseRepository = recurringExpenseRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(p => p.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(p => p).MustAsync((recurringExpense, _) => ValidCreditCard(recurringExpense)).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
            RuleFor(p => p).MustAsync((recurringExpense, _) => ValidRecurringExpense(recurringExpense)).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
        }

        private async Task<bool> ValidCreditCard(RecurringExpenseEntity recurringExpense)
        {
            if (recurringExpense.CreditCardId > 0)
            {
                var cards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = recurringExpense.UserId });
                var card = cards.FirstOrDefault(p => p.Id == recurringExpense.CreditCardId.Value);
                if (card == null || card.UserId != recurringExpense.UserId)
                    return false;
            }
            else
                recurringExpense.CreditCardId = null;
            return true;
        }

        private async Task<bool> ValidRecurringExpense(RecurringExpenseEntity recurringExpense)
        {
            var recurringExpenseDb = await _recurringExpenseRepository.GetById(recurringExpense.Id);
            return recurringExpenseDb?.UserId == recurringExpense.UserId;
        }
    }
}