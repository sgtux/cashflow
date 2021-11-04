using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class DailyExpensesService : BaseService
    {
        private IDailyExpensesRepository _dailyExpensesRepository;

        public DailyExpensesService(IDailyExpensesRepository dailyExpensesRepository) => _dailyExpensesRepository = dailyExpensesRepository;

        public async Task<ResultDataModel<IEnumerable<DailyExpenses>>> GetByUser(int userId, int month, int year)
        {
            var now = DateTime.Now;

            if (month > 12 || month < 1)
                month = now.Month;

            if (year > now.Year + 5 || year < now.Year - 5)
                year = now.Year;

            var list = await _dailyExpensesRepository.GetByUser(userId);
            list = list.Where(p => p.Date.Month == month && p.Date.Year == year);
            return new ResultDataModel<IEnumerable<DailyExpenses>>(list);
        }

        public async Task<ResultDataModel<DailyExpenses>> GetById(long id, int userId)
        {
            var p = await _dailyExpensesRepository.GetById(id);
            return new ResultDataModel<DailyExpenses>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultModel> Add(DailyExpenses dailyExpenses)
        {
            var result = new ResultModel();
            var validatorResult = new DailyExpensesValidator(_dailyExpensesRepository).Validate(dailyExpenses);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
                await _dailyExpensesRepository.Add(dailyExpenses);
            return result;
        }

        public async Task<ResultModel> Update(DailyExpenses dailyExpenses)
        {
            var result = new ResultModel();
            var validatorResult = new DailyExpensesValidator(_dailyExpensesRepository).Validate(dailyExpenses);
            if (validatorResult.IsValid)
                await _dailyExpensesRepository.Update(dailyExpenses);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int salaryId, int userId)
        {
            var result = new ResultModel();

            var salary = await _dailyExpensesRepository.GetById(salaryId);
            if (salary is null || salary.UserId != userId)
                result.AddNotification(ValidatorMessages.Salary.NotFound);
            else
                await _dailyExpensesRepository.Remove(salaryId);
            return result;
        }
    }
}