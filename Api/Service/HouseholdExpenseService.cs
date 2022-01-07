using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class HouseholdExpenseService : BaseService
    {
        private IHouseholdExpenseRepository _householdExpenseRepository;

        public HouseholdExpenseService(IHouseholdExpenseRepository householdExpenseRepository) => _householdExpenseRepository = householdExpenseRepository;

        public async Task<ResultDataModel<IEnumerable<HouseholdExpense>>> GetByUser(int userId, int month, int year)
        {
            var now = DateTime.Now;

            if (month > 12 || month < 1)
                month = now.Month;

            if (year > now.Year + 5 || year < now.Year - 5)
                year = now.Year;

            var list = await _householdExpenseRepository.GetSome(new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            });
            return new ResultDataModel<IEnumerable<HouseholdExpense>>(list);
        }

        public async Task<ResultDataModel<HouseholdExpense>> GetById(long id, int userId)
        {
            var p = await _householdExpenseRepository.GetById(id);
            return new ResultDataModel<HouseholdExpense>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultModel> Add(HouseholdExpense householdExpense)
        {
            var result = new ResultModel();
            var validatorResult = new HouseholdExpenseValidator(_householdExpenseRepository).Validate(householdExpense);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
                await _householdExpenseRepository.Add(householdExpense);
            return result;
        }

        public async Task<ResultModel> Update(HouseholdExpense householdExpense)
        {
            var result = new ResultModel();
            var validatorResult = new HouseholdExpenseValidator(_householdExpenseRepository).Validate(householdExpense);
            if (validatorResult.IsValid)
                await _householdExpenseRepository.Update(householdExpense);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();

            var householdExpense = await _householdExpenseRepository.GetById(id);
            if (householdExpense is null || householdExpense.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Despesa Doméstica"));
            else
                await _householdExpenseRepository.Remove(id);
            return result;
        }
    }
}