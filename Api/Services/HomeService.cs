using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared.Cache;

namespace Cashflow.Api.Services
{
    public class HomeService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

        private readonly IEarningRepository _earningRepository;

        private readonly AppCache _appCache;

        public HomeService(IPaymentRepository paymentRepository,
            ICreditCardRepository creditCardRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IEarningRepository earningRepository,
            AppCache appCache
            )
        {
            _paymentRepository = paymentRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
            _earningRepository = earningRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<List<HomeChartModel>>> GetInfo(int userId, short month, short year)
        {
            var list = _appCache.Home.Get(userId);

            if (list != null && list.First().Month == month && list.First().Year == year)
                return new ResultDataModel<List<HomeChartModel>>(list, true);

            list = new List<HomeChartModel>();

            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            };

            var householdExpenseModel = new HomeChartModel() { Index = 0, Description = "Despesas Domésticas" };
            var spendingModel = new HomeChartModel() { Index = 1, Description = "Outros Gastos" };
            var vehicleModel = new HomeChartModel() { Index = 2, Description = "Combustível" };
            var financingsModel = new HomeChartModel() { Index = 3, Description = "Financiamentos" };
            var loanModel = new HomeChartModel() { Index = 4, Description = "Empréstimos" };
            var donationModel = new HomeChartModel() { Index = 5, Description = "Doações" };
            var educationModel = new HomeChartModel() { Index = 6, Description = "Educação" };
            var earningsModel = new HomeChartModel() { Index = 7, Description = "Provento" };

            foreach (var item in await _vehicleRepository.GetSome(filter))
                vehicleModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in await _householdExpenseRepository.GetSome(filter))
                householdExpenseModel.Value += item.Value;

            var payments = await _paymentRepository.GetSome(filter);

            earningsModel.Value = (await _earningRepository.GetSome(filter)).Sum(p => p.Value);

            spendingModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Spending);
            donationModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Donation);
            financingsModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Financing);
            educationModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Education);
            loanModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Loan);

            list.Add(spendingModel);
            list.Add(householdExpenseModel);
            list.Add(vehicleModel);
            list.Add(financingsModel);
            list.Add(loanModel);
            list.Add(donationModel);
            list.Add(educationModel);
            list.Add(earningsModel);

            foreach (var item in list)
            {
                item.Month = month;
                item.Year = year;
            }

            _appCache.Home.Update(userId, list);

            return new ResultDataModel<List<HomeChartModel>>(list);
        }

        private decimal CalculatePaymentHomeChartModel(IEnumerable<Payment> payments, int month, int year, Enums.PaymentType type)
        {
            decimal value = 0;
            foreach (var item in payments.Where(p => p.Type == type))
                value += item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month).Sum(p => p.Value);
            return value;
        }
    }
}