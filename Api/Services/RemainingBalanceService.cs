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

namespace Cashflow.Api.Services
{
    public class RemainingBalanceService : BaseService
    {
        private IRemainingBalanceRepository _remainingBalanceRepository;

        private IVehicleRepository _vehicleRepository;

        private IHouseholdExpenseRepository _householdExpenseRepository;

        private IPaymentRepository _paymentRepository;

        private IEarningRepository _earningRepository;

        private IRecurringExpenseRepository _recurringExpenseRepository;

        public RemainingBalanceService(IRemainingBalanceRepository remainingBalanceRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IPaymentRepository paymentRepository,
            IEarningRepository earningRepository,
            IRecurringExpenseRepository recurringExpenseRepository)
        {
            _remainingBalanceRepository = remainingBalanceRepository;
            _vehicleRepository = vehicleRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _paymentRepository = paymentRepository;
            _earningRepository = earningRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
        }

        public async Task<ResultModel> GetAll(int userId) => new ResultDataModel<IEnumerable<RemainingBalance>>(await _remainingBalanceRepository.GetSome(new BaseFilter() { UserId = userId }));

        public async Task<ResultModel> Recalculate(int userId, DateTime date, bool force = false)
        {
            var resultModel = new ResultModel();
            var current = await _remainingBalanceRepository.GetByMonthYear(userId, date);
            if (current != null && !force)
                return resultModel;

            date = date.AddMonths(-1);
            var filter = new BaseFilter()
            {
                StartDate = new DateTime(date.Year, date.Month, 1).FixStartTimeFilter(),
                EndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).FixEndTimeFilter(),
                UserId = userId
            };

            filter.InactiveTo = filter.EndDate?.FixEndTimeFilter();

            decimal total = 0;

            foreach (var item in (await _earningRepository.GetSome(filter)))
                total += item.Value;

            foreach (var item in (await _vehicleRepository.GetSome(filter)))
                total -= item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in (await _householdExpenseRepository.GetSome(filter)))
                total -= item.Value;

            foreach (var item in (await _paymentRepository.GetSome(filter)))
            {
                if (item.Type.In)
                    total += item.Installments.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Cost);
                else
                    total -= item.Installments.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Cost);
            }

            var lastRemainingBalance = await _remainingBalanceRepository.GetByMonthYear(userId, date);
            if (lastRemainingBalance != null)
                total += lastRemainingBalance.Value;

            foreach (var item in (await _recurringExpenseRepository.GetSome(filter)))
                total -= item.History.First().PaidValue;

            date = date.AddMonths(1);

            if (current != null)
            {
                current.Value = total;
                await _remainingBalanceRepository.Update(current);
            }
            else
            {
                await _remainingBalanceRepository.Add(new Infra.Entity.RemainingBalance()
                {
                    Value = total,
                    Month = date.Month,
                    Year = date.Year,
                    UserId = userId
                });
            }

            return resultModel;
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
            return result;
        }
    }
}