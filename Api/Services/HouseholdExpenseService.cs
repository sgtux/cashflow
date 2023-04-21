using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class HouseholdExpenseService : BaseService
    {
        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

        private readonly AppCache _appCache;

        public HouseholdExpenseService(IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            AppCache appCache)
        {
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<IEnumerable<HouseholdExpense>>> GetByUser(int userId, int month, int year)
        {
            var now = CurrentDate;

            if (month > 12 || month < 1)
                month = now.Month;

            if (year > now.Year + 5 || year < now.Year - 5)
                year = now.Year;

            var list = await _householdExpenseRepository.GetSome(new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1).FixStartTimeFilter(),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).FixEndTimeFilter(),
                UserId = userId
            });
            return new ResultDataModel<IEnumerable<HouseholdExpense>>(list.OrderByDescending(p => p.Date));
        }

        public async Task<ResultDataModel<HouseholdExpense>> GetById(long id, int userId)
        {
            var p = await _householdExpenseRepository.GetById(id);
            return new ResultDataModel<HouseholdExpense>(p?.UserId == userId ? p : null);
        }

        public ResultDataModel<IEnumerable<TypeModel>> GetTypes()
        {
            var types = Enum.GetValues<HouseholdExpenseType>()
                .Where(p => p != HouseholdExpenseType.Others)
                .FilterInactives()
                .Select(p => new TypeModel(p))
                .OrderBy(p => p.Description)
                .Append(new TypeModel(HouseholdExpenseType.Others));
            return new ResultDataModel<IEnumerable<TypeModel>>(types);
        }

        public async Task<ResultModel> Add(HouseholdExpense householdExpense)
        {
            var result = new ResultModel();
            var validatorResult = new HouseholdExpenseValidator(_householdExpenseRepository, _vehicleRepository).Validate(householdExpense);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
            {
                await _householdExpenseRepository.Add(householdExpense);
                _appCache.Clear(householdExpense.UserId);
            }
            return result;
        }

        public async Task<ResultModel> Update(HouseholdExpense householdExpense)
        {
            var result = new ResultModel();
            var validatorResult = new HouseholdExpenseValidator(_householdExpenseRepository, _vehicleRepository).Validate(householdExpense);
            if (validatorResult.IsValid)
            {
                await _householdExpenseRepository.Update(householdExpense);
                _appCache.Clear(householdExpense.UserId);
            }
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
            {
                await _householdExpenseRepository.Remove(id);
                _appCache.Clear(householdExpense.UserId);
            }
            return result;
        }
    }
}