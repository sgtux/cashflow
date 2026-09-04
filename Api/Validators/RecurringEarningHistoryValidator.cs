using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class RecurringEarningHistoryValidator : AbstractValidator<RecurringEarningHistoryEntity>
    {
        private readonly IRecurringEarningRepository _recurringEarningRepository;

        private RecurringEarningEntity _recurringEarning;

        private int _userId;

        public RecurringEarningHistoryValidator(IRecurringEarningRepository recurringEarningRepository, int userId)
        {
            _recurringEarningRepository = recurringEarningRepository;
            _userId = userId;
            RuleFor(p => p.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.FieldIsRequired("Valor"));
            RuleFor(p => p).MustAsync((history, _) => ValidMonthYear(history)).WithMessage("Já existe um histórico para este Mês/Ano.");
            RuleFor(p => p).MustAsync((history, _) => ValidRecurringEarning(history)).WithMessage(ValidatorMessages.NotFound("Provento Recorrente"));
            RuleFor(p => p).MustAsync((history, _) => ValidRecurringEarning(history)).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Provento Recorrente"));
        }

        private async Task<bool> ValidMonthYear(RecurringEarningHistoryEntity history)
        {
            await LoadRecurringEarning(history.RecurringEarningId);
            return _recurringEarning?.History != null && !_recurringEarning.History.Any(p => p.Id != history.Id && p.Date.SameMonthYear(history.Date));
        }

        private async Task<bool> ValidRecurringEarning(RecurringEarningHistoryEntity history)
        {
            await LoadRecurringEarning(history.RecurringEarningId);
            return _recurringEarning?.UserId == _userId;
        }

        private async Task LoadRecurringEarning(long id)
        {
            if (_recurringEarning == null)
                _recurringEarning = await _recurringEarningRepository.GetById(id);
        }
    }
}
