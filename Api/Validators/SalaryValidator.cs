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
            RuleFor(s => s.StartDate).NotEqual(default(System.DateTime)).WithMessage("A Data de Início é inválida.");
            RuleFor(s => s.EndDate).NotEqual(default(System.DateTime)).WithMessage("A Data Fim é inválida."); ;
            RuleFor(s => s.Value).GreaterThan(0).WithMessage("O valor deve ser maior que zero");
            RuleFor(s => s.EndDate <= s.StartDate).NotEqual(true).WithMessage("A Data Início não pode ser maior que a Data Fim.");
            RuleFor(s => ValidateCurretSalary(s)).NotEqual(true).WithMessage("Tem outro salário vigente.");
            RuleFor(s => ValidateIntervalDate(s)).NotEqual(true).WithMessage("Tem outro salário neste intervalo de datas.");
        }

        public bool ValidateCurretSalary(Salary salary)
        {
            LoadSalaries(salary);
            return salary.EndDate is null && _salaries.Any(p => p.EndDate is null && p.Id != salary.Id);
        }

        public bool ValidateIntervalDate(Salary salary)
        {
            LoadSalaries(salary);
            if (_salaries.Any(p => salary.StartDate >= p.StartDate && salary.StartDate <= p.EndDate && p.Id != salary.Id))
                return true;
            return _salaries.Any(p => salary.EndDate >= p.StartDate && salary.EndDate <= p.EndDate && p.Id != salary.Id);
        }

        public IEnumerable<Salary> LoadSalaries(Salary salary)
        {
            if (_salaries is null)
                _salaries = _repository.GetByUserId(salary.UserId).Result;
            return _salaries;
        }
    }
}