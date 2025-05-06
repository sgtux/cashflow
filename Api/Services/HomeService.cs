using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
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

        private readonly IUserRepository _userRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

        private readonly IEarningRepository _earningRepository;

        private readonly AppCache _appCache;

        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        public HomeService(IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            IRecurringExpenseRepository recurringExpenseRepository,
            IEarningRepository earningRepository,
            AppCache appCache
            )
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
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

            homeModel = new HomeModel(month, year);

            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1).FixStartTimeFilter(),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).FixEndTimeFilter(),
                UserId = userId
            };

            var user = await _userRepository.GetById(userId);

            var householdExpenseInflowOutflowModel = new InflowOutflowModel("Despesas Domésticas");
            var paymentInflowOutflowModel = new InflowOutflowModel("Parcelamentos");
            var vehicleInflowOutflowModel = new InflowOutflowModel("Combustível");
            var recurringExpenseInflowOutflowModel = new InflowOutflowModel("Despesas Recorrentes");

            foreach (var item in await _vehicleRepository.GetSome(filter))
                vehicleInflowOutflowModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);
            if (vehicleInflowOutflowModel.Value == 0)
                vehicleInflowOutflowModel.Value = user.FuelExpenseLimit;

            var householdExpenses = await _householdExpenseRepository.GetSome(new HouseholdExpenseFilter()
            {
                StartDate = new DateTime(year, month, 1).AddMonths(-1).FixStartTimeFilter(),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).FixEndTimeFilter(),
                UserId = userId
            });
            householdExpenseInflowOutflowModel.Value = householdExpenses.Where(p => p.InvoiceDate.SameMonthYear(filter.StartDate.Value)).Sum(p => p.Value);
            if (householdExpenseInflowOutflowModel.Value == 0)
                householdExpenseInflowOutflowModel.Value = user.ExpenseLimit;

            var payments = await _paymentRepository.GetSome(new PaymentFilter()
            {
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                UserId = filter.UserId
            });
            foreach (var item in payments)
            {
                var installments = item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month);
                paymentInflowOutflowModel.Value += installments.Sum(p => p.Value);

                var pendingValue = installments.Where(p => !p.PaidValue.HasValue).Sum(p => p.Value);
                if (pendingValue > 0)
                    homeModel.PendingPayments.Add(new PendingPaymentModel($"{item.Description} (Parcelado)", pendingValue));
            }

            var recurringExpenses = await _recurringExpenseRepository.GetSome(new RecurringExpenseFilter() { UserId = userId, Active = 1 });
            foreach (var item in recurringExpenses)
            {
                recurringExpenseInflowOutflowModel.Value += item.Value;
                if (!item.Paid)
                    homeModel.PendingPayments.Add(new PendingPaymentModel($"{item.Description} (Recorrente)", item.Value));
            }

            foreach (var item in await _earningRepository.GetSome(filter))
                homeModel.Inflows.Add(new InflowOutflowModel(item.Description, item.Value));

            homeModel.Outflows.Add(householdExpenseInflowOutflowModel);
            homeModel.Outflows.Add(paymentInflowOutflowModel);
            homeModel.Outflows.Add(vehicleInflowOutflowModel);
            homeModel.Outflows.Add(recurringExpenseInflowOutflowModel);

            homeModel.ChartInfos.ForEach(p => homeModel.Outflows.Add(new InflowOutflowModel(p.Description, p.Value)));

            FillLimitValues(homeModel, user, householdExpenseInflowOutflowModel, vehicleInflowOutflowModel);

            _appCache.Home.Update(userId, homeModel);

            return new ResultDataModel<HomeModel>(homeModel);
        }

        private void FillLimitValues(HomeModel homeModel, User user, InflowOutflowModel householdExpenseModel, InflowOutflowModel vehicleModel)
        {
            if (user.ExpenseLimit > 0)
                homeModel.LimitValues.Add(new LimitValueModel()
                {
                    Description = "Despesas Domésticas",
                    Limit = user.ExpenseLimit,
                    Spent = householdExpenseModel.Value
                });
            if (user.FuelExpenseLimit > 0)
                homeModel.LimitValues.Add(new LimitValueModel()
                {
                    Description = "Combustível",
                    Limit = user.FuelExpenseLimit,
                    Spent = vehicleModel.Value
                });
        }
    }
}