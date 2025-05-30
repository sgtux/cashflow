using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;
using System.Collections.Generic;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Utils;

namespace Cashflow.Api.Services
{
    public class RemainingBalanceService : BaseService
    {
        private readonly IRemainingBalanceRepository _remainingBalanceRepository;

        private readonly IVehicleRepository _vehicleRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IPaymentRepository _paymentRepository;

        private readonly IEarningRepository _earningRepository;

        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private readonly AppCache _appCache;

        public RemainingBalanceService(IRemainingBalanceRepository remainingBalanceRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IPaymentRepository paymentRepository,
            IEarningRepository earningRepository,
            IRecurringExpenseRepository recurringExpenseRepository,
            AppCache appCache)
        {
            _remainingBalanceRepository = remainingBalanceRepository;
            _vehicleRepository = vehicleRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _paymentRepository = paymentRepository;
            _earningRepository = earningRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
            _appCache = appCache;
        }

        public async Task<ResultModel> GetAll(int userId)
        {
            var list = (await _remainingBalanceRepository.GetSome(new BaseFilter() { UserId = userId })).ToList();
            var result = await Recalculate(userId, DateTimeUtils.CurrentDate, false, true);
            list.Add(result.Data);
            return new ResultDataModel<IEnumerable<RemainingBalance>>(list.OrderByDescending(p => p.Date));
        }

        public async Task<ResultDataModel<RemainingBalance>> Recalculate(int userId, DateTime date, bool force = false, bool simulation = false)
        {
            var current = await _remainingBalanceRepository.GetByMonthYear(userId, date);
            if (current != null && !force)
                return new ResultDataModel<RemainingBalance>(current);

            var filter = new RecurringExpenseFilter()
            {
                StartDate = date.FixFirstDayInMonth().FixStartTimeFilter(),
                EndDate = date.FixLastDayInMonth().FixEndTimeFilter(),
                UserId = userId
            };

            filter.InactiveTo = filter.EndDate?.FixEndTimeFilter();

            decimal total = 0;

            foreach (var item in await _earningRepository.GetSome(filter))
                total += item.Value;

            total -= await CalculateFuelExpenses(filter);

            total -= await CalculateHouseholdExpenses(filter);

            foreach (var item in await _paymentRepository.GetSome(new PaymentFilter() { UserId = filter.UserId }))
                total -= item.Installments.Where(p => p.PaidDate?.SameMonthYear(date) ?? false).Sum(p => p.PaidValue.Value);


            var lastRemainingBalance = await _remainingBalanceRepository.GetByMonthYear(userId, date.AddMonths(-1));
            if (lastRemainingBalance != null)
                total += lastRemainingBalance.Value;

            foreach (var item in await _recurringExpenseRepository.GetSome(filter))
                total -= item.History.First().PaidValue;

            var newRemainingBalance = new RemainingBalance()
            {
                Value = total,
                Month = date.Month,
                Year = date.Year,
                UserId = userId
            };

            if (simulation || date.SameMonthYear(DateTimeUtils.CurrentDate))
                return new ResultDataModel<RemainingBalance>(newRemainingBalance);

            if (current == null)
            {
                await _remainingBalanceRepository.Add(newRemainingBalance);
                _appCache.Clear(userId);
                return new ResultDataModel<RemainingBalance>(newRemainingBalance);
            }

            current.Value = total;
            await _remainingBalanceRepository.Update(current);
            _appCache.Clear(userId);
            return new ResultDataModel<RemainingBalance>(current);
        }

        public async Task<ResultModel> Update(int userId, RemainingBalanceModel model)
        {
            var result = new ResultModel();
            var remainingBalance = await _remainingBalanceRepository.GetByMonthYear(userId, new DateTime(model.Year, model.Month, 1));
            if (remainingBalance == null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Saldo Remanecente"));
                return result;
            }
            remainingBalance.Value = model.Value;
            await _remainingBalanceRepository.Update(remainingBalance);
            _appCache.Clear(userId);
            return result;
        }

        private async Task<decimal> CalculateFuelExpenses(BaseFilter filter)
        {
            decimal total = 0;
            var vehicles = await _vehicleRepository.GetSome(new BaseFilter()
            {
                UserId = filter.UserId,
                StartDate = filter.StartDate.Value.FixStartTimeFilter(),
                EndDate = filter.EndDate.FixEndTimeFilter()
            });

            foreach (var item in vehicles)
                total += item.FuelExpenses.Sum(p => p.ValueSupplied);

            return total;
        }

        private async Task<decimal> CalculateHouseholdExpenses(BaseFilter filter)
        {
            var expenses = await _householdExpenseRepository.GetSome(new HouseholdExpenseFilter()
            {
                UserId = filter.UserId,
                StartDate = filter.StartDate.Value.AddMonths(-1),
                EndDate = filter.EndDate
            });
            return expenses.Where(p => p.CreditCardId.HasValue ? p.InvoiceDate.SameMonthYear(filter.StartDate.Value) : p.Date.SameMonthYear(filter.StartDate.Value)).Sum(p => p.Value);
        }
    }
}