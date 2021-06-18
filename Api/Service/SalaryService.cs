using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class SalaryService : BaseService
    {
        private ISalaryRepository _salaryRepository;

        public SalaryService(ISalaryRepository salaryRepository) => _salaryRepository = salaryRepository;

        public async Task<ResultModel> GetByUser(int userId) => new ResultDataModel<IEnumerable<Salary>>(await _salaryRepository.GetByUserId(userId));

        public async Task<ResultModel> Add(Salary salary)
        {
            var result = new ResultModel();
            salary.SetDays();
            var validatorResult = new SalaryValidator(_salaryRepository).Validate(salary);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
                await _salaryRepository.Add(salary);
            return result;
        }

        public async Task<ResultModel> Update(Salary salary)
        {
            var result = new ResultModel();
            salary.SetDays();
            var validatorResult = new SalaryValidator(_salaryRepository).Validate(salary);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
            {
                var salaryDb = await _salaryRepository.GetById(salary.Id);
                if (salaryDb is null || salaryDb.UserId != salary.UserId)
                    result.AddNotification("Salário não encontrado.");
                else
                {
                    salary.Map(salaryDb);
                    await _salaryRepository.Update(salaryDb);
                }
            }
            return result;
        }

        public async Task<ResultModel> Remove(int salaryId, int userId)
        {
            var result = new ResultModel();

            var salary = await _salaryRepository.GetById(salaryId);
            if (salary is null || salary.UserId != userId)
                result.AddNotification("Payment not found.");
            else
                await _salaryRepository.Remove(salaryId);
            return result;
        }
    }
}