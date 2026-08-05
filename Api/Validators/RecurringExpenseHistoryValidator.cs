using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class RecurringExpenseHistoryValidator : AbstractValidator<RecurringExpenseHistoryEntity>
    {
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private RecurringExpenseEntity _recurringExpense;

        private int _userId;

        public RecurringExpenseHistoryValidator(IRecurringExpenseRepository recurringExpenseRepository, int userId)
        {
            _recurringExpenseRepository = recurringExpenseRepository;
            _userId = userId;
            RuleFor(p => p.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.PaidValue).GreaterThan(0).WithMessage(ValidatorMessages.FieldIsRequired("Valor Pago"));
            RuleFor(p => p).MustAsync((history, _) => ValidMonthYear(history)).WithMessage("Já existe um histórico para este Mês/Ano.");
            RuleFor(p => p).MustAsync((history, _) => ValidRecurringExpense(history)).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
            RuleFor(p => p).MustAsync((history, _) => ValidRecurringExpense(history)).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
        }

        private async Task<bool> ValidMonthYear(RecurringExpenseHistoryEntity history)
        {
            await LoadRecurringExpense(history.RecurringExpenseId);
            return _recurringExpense?.History != null && !_recurringExpense.History.Any(p => p.Id != history.Id && p.Date.SameMonthYear(history.Date));
        }

        private async Task<bool> ValidRecurringExpense(RecurringExpenseHistoryEntity history)
        {
            await LoadRecurringExpense(history.RecurringExpenseId);
            return _recurringExpense?.UserId == _userId;
        }

        private async Task LoadRecurringExpense(long id)
        {
            if (_recurringExpense == null)
                _recurringExpense = await _recurringExpenseRepository.GetById(id);
        }
    }
}