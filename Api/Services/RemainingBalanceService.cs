using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Extensions;
using System.Text.Json;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Services
{
    public class RemainingBalanceService : BaseService
    {
        private IRemainingBalanceRepository _remainingBalanceRepository;

        private IVehicleRepository _vehicleRepository;

        private IHouseholdExpenseRepository _householdExpenseRepository;

        private IPaymentRepository _paymentRepository;

        private ISalaryRepository _salaryRepository;

        private IRecurringExpenseRepository _recurringExpenseRepository;

        public RemainingBalanceService(IRemainingBalanceRepository remainingBalanceRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IPaymentRepository paymentRepository,
            ISalaryRepository salaryRepository,
            IRecurringExpenseRepository recurringExpenseRepository)
        {
            _remainingBalanceRepository = remainingBalanceRepository;
            _vehicleRepository = vehicleRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _paymentRepository = paymentRepository;
            _salaryRepository = salaryRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
        }

        public async Task Update(int userId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var filter = new BaseFilter()
            {
                StartDate = new DateTime(date.Year, date.Month, 1).FixStartTimeFilter(),
                EndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).FixEndTimeFilter(),
                UserId = userId
            };

            filter.InactiveTo = filter.EndDate?.FixEndTimeFilter();

            Salary salary = null;
            decimal total;

            var salaries = await _salaryRepository.GetSome(filter);
            salary = salaries.FirstOrDefault(p => date.SameMonthYear(p.EndDate ?? default(DateTime)));
            if (salary == null)
                total = salaries.FirstOrDefault(p => p.EndDate == null)?.Value ?? 0;
            else
                total = salary.Value;

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

            var lastRemainingBalance = await _remainingBalanceRepository.GetByMonthYear(userId, date.Month, date.Year);
            if (lastRemainingBalance != null)
                total += lastRemainingBalance.Value;

            foreach (var item in (await _recurringExpenseRepository.GetSome(filter)))
                total -= item.History.First().PaidValue;

            date = date.AddMonths(1);

            var current = await _remainingBalanceRepository.GetByMonthYear(userId, date.Month, date.Year);
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
        }
    }
}