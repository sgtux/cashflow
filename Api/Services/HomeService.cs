using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Models.Home;
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

        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        public HomeService(IPaymentRepository paymentRepository,
            ICreditCardRepository creditCardRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IRecurringExpenseRepository recurringExpenseRepository,
            IEarningRepository earningRepository,
            AppCache appCache
            )
        {
            _paymentRepository = paymentRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
            _earningRepository = earningRepository;
            _appCache = appCache;
            _recurringExpenseRepository = recurringExpenseRepository;
        }

        public async Task<ResultDataModel<HomeModel>> GetInfo(int userId, short month, short year)
        {
            var homeModel = _appCache.Home.Get(userId);

            if (homeModel != null && homeModel.Month == month && homeModel.Year == year)
                return new ResultDataModel<HomeModel>(homeModel, true);

            homeModel = new HomeModel();

            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            };

            var householdExpenseModel = new ChartModel() { Index = 0, Description = "Despesas Domésticas", Month = month, Year = year };
            var paymentModel = new ChartModel() { Index = 1, Description = "Parcelamentos", Month = month, Year = year };
            var vehicleModel = new ChartModel() { Index = 2, Description = "Combustível", Month = month, Year = year };
            var recurringExpenseModel = new ChartModel() { Index = 3, Description = "Despesas Recorrentes", Month = month, Year = year };
            var earningsModel = new ChartModel() { Index = 4, Description = "Provento", Month = month, Year = year };

            foreach (var item in await _vehicleRepository.GetSome(filter))
                vehicleModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in await _householdExpenseRepository.GetSome(filter))
                householdExpenseModel.Value += item.Value;

            var payments = await _paymentRepository.GetSome(filter);
            foreach (var item in payments)
            {
                var installments = item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month);
                paymentModel.Value += installments.Sum(p => p.Value);

                var pendingValue = installments.Where(p => !p.PaidValue.HasValue).Sum(p => p.Value);
                if (pendingValue > 0)
                    homeModel.PendingPayments.Add(new PendingPaymentModel($"{item.Description} (Parcelado)", pendingValue));
            }

            var recurringExpenses = await _recurringExpenseRepository.GetSome(new BaseFilter() { UserId = userId, Active = 1 });
            foreach (var item in recurringExpenses)
            {
                recurringExpenseModel.Value += item.Value;
                if (!item.Paid)
                    homeModel.PendingPayments.Add(new PendingPaymentModel($"{item.Description} (Recorrente)", item.Value));
            }

            earningsModel.Value = (await _earningRepository.GetSome(filter)).Sum(p => p.Value);

            homeModel.ChartInfos.Add(paymentModel);
            homeModel.ChartInfos.Add(householdExpenseModel);
            homeModel.ChartInfos.Add(vehicleModel);
            homeModel.ChartInfos.Add(paymentModel);
            homeModel.ChartInfos.Add(recurringExpenseModel);
            homeModel.ChartInfos.Add(earningsModel);

            _appCache.Home.Update(userId, homeModel);

            return new ResultDataModel<HomeModel>(homeModel);
        }
    }
}