using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Service
{
    public class RemainingBalanceService : BaseService
    {
        private IRemainingBalanceRepository _remainingBalanceRepository;

        private IVehicleRepository _vehicleRepository;

        private IDailyExpensesRepository _dailyExpensesRepository;

        private IPaymentRepository _paymentRepository;

        private ISalaryRepository _salaryRepository;

        public RemainingBalanceService(IRemainingBalanceRepository remainingBalanceRepository,
            IDailyExpensesRepository dailyExpensesRepository,
            IVehicleRepository vehicleRepository,
            IPaymentRepository paymentRepository,
            ISalaryRepository salaryRepository)
        {
            _remainingBalanceRepository = remainingBalanceRepository;
            _vehicleRepository = vehicleRepository;
            _dailyExpensesRepository = dailyExpensesRepository;
            _paymentRepository = paymentRepository;
            _salaryRepository = salaryRepository;
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

            decimal total = (await _salaryRepository.GetSome(filter)).FirstOrDefault(p => p.EndDate == null)?.Value ?? 0;

            foreach (var item in (await _vehicleRepository.GetSome(filter)))
                total -= item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in (await _dailyExpensesRepository.GetSome(filter)))
                total -= item.TotalPrice;

            foreach (var item in (await _paymentRepository.GetSome(filter)))
            {
                if (item.Type.In)
                    total += item.Installments.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Cost);
                else
                    total -= item.Installments.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Cost);
            }

            var current = await _remainingBalanceRepository.GetByMonthYear(userId, date.Month + 1, date.Year);
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
                    Month = date.Month + 1,
                    Year = date.Year,
                    UserId = userId
                });
            }
        }
    }
}