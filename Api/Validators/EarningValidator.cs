using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class EarningValidator : AbstractValidator<Earning>
    {
        private readonly IEarningRepository _repository;

        private IEnumerable<Earning> _earnings;

        public EarningValidator(IEarningRepository repository)
        {
            _repository = repository;
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(s => s.Type).IsInEnum().WithMessage("Tipo inválido.");
            RuleFor(s => s).Must(EarningExists).When(p => p.Id > 0).WithMessage(ValidatorMessages.NotFound("Benefício/Salário"));
        }

        private bool EarningExists(Earning earning)
        {
            LoadEarnings(earning);
            return _earnings.Any(p => p.Id == earning.Id && p.UserId == earning.UserId);
        }

        private void LoadEarnings(Earning earning)
        {
            if (_earnings is null)
                _earnings = _repository.GetSome(new BaseFilter() { UserId = earning.UserId }).Result;
        }
    }
}