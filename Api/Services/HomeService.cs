using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;

namespace Cashflow.Api.Services
{
    public class HomeService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

        public HomeService(IPaymentRepository paymentRepository,
            ICreditCardRepository creditCardRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            EarningService earningService
            )
        {
            _paymentRepository = paymentRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ResultDataModel<List<HomeChartModel>>> GetInfo(int userId, int month, int year)
        {
            var result = new ResultDataModel<List<HomeChartModel>>();
            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            };

            var householdExpenseModel = new HomeChartModel() { Index = 0, Description = "Despesas Domésticas" };
            var expensesModel = new HomeChartModel() { Index = 1, Description = "Outros Gastos" };
            var vehicleModel = new HomeChartModel() { Index = 2, Description = "Combustível" };
            var financingsModel = new HomeChartModel() { Index = 3, Description = "Financiamentos" };
            var loanModel = new HomeChartModel() { Index = 4, Description = "Empréstimos" };
            var contributionsModel = new HomeChartModel() { Index = 5, Description = "Aportes (Investimentos)" };
            var educationModel = new HomeChartModel() { Index = 6, Description = "Educação" };
            var earningsModel = new HomeChartModel() { Index = 7, Description = "Benefícios" };

            foreach (var item in (await _vehicleRepository.GetSome(filter)))
                vehicleModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in (await _householdExpenseRepository.GetSome(filter)))
                householdExpenseModel.Value += item.Value;

            var payments = await _paymentRepository.GetSome(filter);

            expensesModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Expense);
            contributionsModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Contributions);
            financingsModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Financing);
            educationModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Education);
            loanModel.Value += CalculatePaymentHomeChartModel(payments, month, year, Enums.PaymentType.Loan);

            result.Data.Add(expensesModel);
            result.Data.Add(householdExpenseModel);
            result.Data.Add(vehicleModel);
            result.Data.Add(financingsModel);
            result.Data.Add(loanModel);
            result.Data.Add(contributionsModel);
            result.Data.Add(educationModel);
            result.Data.Add(earningsModel);

            return result;
        }

        private decimal CalculatePaymentHomeChartModel(IEnumerable<Payment> payments, int month, int year, Enums.PaymentType type)
        {
            decimal value = 0;
            foreach (var item in payments.Where(p => p.TypeId == type))
                value += item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month).Sum(p => p.Value);
            return value;
        }
    }
}