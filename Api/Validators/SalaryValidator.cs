using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class SalaryValidator : AbstractValidator<Salary>
    {
        private ISalaryRepository _repository;

        private IEnumerable<Salary> _salaries;

        public SalaryValidator(ISalaryRepository repository)
        {
            _repository = repository;
            RuleFor(s => s.StartDate).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.Salary.InvalidStartDate);
            RuleFor(s => s.EndDate).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.Salary.InvalidEndDate); ;
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.Salary.ValueMustBeMoreThenZero);
            RuleFor(s => s.EndDate <= s.StartDate).NotEqual(true).WithMessage(ValidatorMessages.Salary.EndDateMustBeMoreThenStartDate);
            RuleFor(s => s).Must(SalaryExists).When(p => p.Id > 0).WithMessage(ValidatorMessages.Salary.NotFound);
            RuleFor(s => ValidateCurretSalary(s)).NotEqual(true).WithMessage(ValidatorMessages.Salary.AnotherCurrentSalary);
            RuleFor(s => ValidateIntervalDate(s)).NotEqual(true).WithMessage(ValidatorMessages.Salary.AnotherSalaryInThisDateRange);
        }

        private bool ValidateCurretSalary(Salary salary)
        {
            LoadSalaries(salary);
            return salary.EndDate is null && _salaries.Any(p => p.EndDate is null && p.Id != salary.Id);
        }

        private bool ValidateIntervalDate(Salary salary)
        {
            LoadSalaries(salary);
            if (_salaries.Any(p => salary.StartDate >= p.StartDate && salary.StartDate <= p.EndDate && p.Id != salary.Id))
                return true;
            return _salaries.Any(p => salary.EndDate >= p.StartDate && salary.EndDate <= p.EndDate && p.Id != salary.Id);
        }

        private bool SalaryExists(Salary salary)
        {
            LoadSalaries(salary);
            return _salaries.Any(p => p.Id == salary.Id && p.UserId == salary.UserId);
        }

        private void LoadSalaries(Salary salary)
        {
            if (_salaries is null)
                _salaries = _repository.GetByUserId(salary.UserId).Result;
        }
    }
}