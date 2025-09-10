using System.Linq;
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
            RuleFor(p => p).Must(ValidMonthYear).WithMessage("Já existe um histórico para este Mês/Ano.");
            RuleFor(p => p).Must(ValidRecurringExpense).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
            RuleFor(p => p).Must(ValidRecurringExpense).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa Recorrente"));
        }

        private bool ValidMonthYear(RecurringExpenseHistoryEntity history)
        {
            LoadRecurringExpense(history.RecurringExpenseId);
            return _recurringExpense?.History != null && !_recurringExpense.History.Any(p => p.Id != history.Id && p.Date.SameMonthYear(history.Date));
        }

        private bool ValidRecurringExpense(RecurringExpenseHistoryEntity history)
        {
            LoadRecurringExpense(history.RecurringExpenseId);
            return _recurringExpense?.UserId == _userId;
        }

        private void LoadRecurringExpense(long id)
        {
            if (_recurringExpense == null)
                _recurringExpense = _recurringExpenseRepository.GetById(id).Result;
        }
    }
}