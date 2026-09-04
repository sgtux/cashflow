using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class EarningValidator : AbstractValidator<EarningEntity>
    {
        private readonly IEarningRepository _repository;

        private IEnumerable<EarningEntity> _earnings;

        public EarningValidator(IEarningRepository repository)
        {
            _repository = repository;
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(s => s).MustAsync((earning, _) => EarningExists(earning)).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Provento"));
        }

        private async Task<bool> EarningExists(EarningEntity earning)
        {
            await LoadEarnings(earning);
            return _earnings.Any(p => p.Id == earning.Id && p.UserId == earning.UserId);
        }

        private async Task LoadEarnings(EarningEntity earning)
        {
            _earnings ??= await _repository.GetSome(new BaseFilter() { UserId = earning.UserId });
        }
    }
}