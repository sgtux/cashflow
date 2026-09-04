using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class RecurringEarningValidator : AbstractValidator<RecurringEarningEntity>
    {
        private readonly IRecurringEarningRepository _recurringEarningRepository;

        public RecurringEarningValidator(IRecurringEarningRepository recurringEarningRepository)
        {
            _recurringEarningRepository = recurringEarningRepository;
            RuleFor(p => p.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(p => p).MustAsync((recurringEarning, _) => ValidRecurringEarning(recurringEarning)).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Provento Recorrente"));
        }

        private async Task<bool> ValidRecurringEarning(RecurringEarningEntity recurringEarning)
        {
            var recurringEarningDb = await _recurringEarningRepository.GetById(recurringEarning.Id);
            return recurringEarningDb?.UserId == recurringEarning.UserId;
        }
    }
}
