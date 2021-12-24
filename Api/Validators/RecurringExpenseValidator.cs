using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class RecurringExpenseValidator : AbstractValidator<RecurringExpense>
    {
        private IRecurringExpenseRepository _recurringExpenseRepository;

        private ICreditCardRepository _creditCardRepository;

        public RecurringExpenseValidator(IRecurringExpenseRepository recurringExpenseRepository, ICreditCardRepository creditCardRepository)
        {
            _recurringExpenseRepository = recurringExpenseRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(p => p.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(p => p).Must(ValidCreditCard).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
            RuleFor(p => p).Must(ValidRecurringExpense).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
        }

        private bool ValidCreditCard(RecurringExpense recurringExpense)
        {
            if (recurringExpense.CreditCardId > 0)
            {
                var card = _creditCardRepository.GetSome(new BaseFilter() { UserId = recurringExpense.UserId }).Result.FirstOrDefault(p => p.Id == recurringExpense.CreditCardId.Value);
                if (card == null || card.UserId != recurringExpense.UserId)
                    return false;
            }
            else
                recurringExpense.CreditCardId = null;
            return true;
        }

        private bool ValidRecurringExpense(RecurringExpense recurringExpense)
        {
            var recurringExpenseDb = _recurringExpenseRepository.GetById(recurringExpense.Id).Result;
            return recurringExpenseDb?.UserId == recurringExpense.UserId;
        }
    }
}