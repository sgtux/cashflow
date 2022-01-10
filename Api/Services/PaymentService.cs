using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Services
{
    public class PaymentService : BaseService
    {
        private IPaymentRepository _paymentRepository;

        private ICreditCardRepository _creditCardRepository;

        private IHouseholdExpenseRepository _householdExpenseRepository;

        private IVehicleRepository _vehicleRepository;

        public PaymentService(IPaymentRepository paymentRepository,
            ICreditCardRepository creditCardRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository
            )
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ResultDataModel<Payment>> Get(int id, int userId)
        {
            var p = await _paymentRepository.GetById(id);
            return new ResultDataModel<Payment>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultDataModel<IEnumerable<Payment>>> GetByUser(int userId, PaymentFilterModel filterModel)
        {
            var filter = new BaseFilter()
            {
                UserId = userId,
                Active = filterModel.Active,
                InactiveFrom = filterModel.InactiveFrom?.FixStartTimeFilter(),
                InactiveTo = filterModel.InactiveTo?.FixEndTimeFilter()
            };
            return new ResultDataModel<IEnumerable<Payment>>(await _paymentRepository.GetSome(filter));
        }

        public async Task<ResultDataModel<IEnumerable<PaymentType>>> GetTypes() => new ResultDataModel<IEnumerable<PaymentType>>(await _paymentRepository.GetTypes());

        public async Task<ResultDataModel<List<HomeChartModel>>> GetHomeChart(int userId, int month, int year)
        {
            var result = new ResultDataModel<List<HomeChartModel>>();
            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            };

            var expensesModel = new HomeChartModel() { Index = 0, Description = "Gastos Essênciais" };
            var householdExpenseModel = new HomeChartModel() { Index = 1, Description = "Despesas Domésticas" };
            var vehicleModel = new HomeChartModel() { Index = 2, Description = "Combustível" };
            var financingsModel = new HomeChartModel() { Index = 3, Description = "Financiamentos" };
            var loanModel = new HomeChartModel() { Index = 4, Description = "Empréstimos" };
            var contributionsModel = new HomeChartModel() { Index = 5, Description = "Aportes (Investimentos)" };
            var educationModel = new HomeChartModel() { Index = 6, Description = "Educação" };

            foreach (var item in (await _vehicleRepository.GetSome(filter)))
                vehicleModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in (await _householdExpenseRepository.GetSome(filter)))
                householdExpenseModel.Value += item.Value;

            var payments = await _paymentRepository.GetSome(filter);

            expensesModel.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Expense);
            contributionsModel.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Contributions);
            financingsModel.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Financing);
            educationModel.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Education);
            loanModel.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Loan);

            result.Data.Add(expensesModel);
            result.Data.Add(householdExpenseModel);
            result.Data.Add(vehicleModel);
            result.Data.Add(financingsModel);
            result.Data.Add(loanModel);
            result.Data.Add(contributionsModel);
            result.Data.Add(educationModel);

            return result;
        }

        public async Task<ResultModel> Add(Payment payment)
        {
            var result = new ResultModel();
            var validatorResult = new PaymentValidator(_paymentRepository, _creditCardRepository).Validate(payment);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
            {
                UpdateMonthlyPayment(payment);
                await _paymentRepository.Add(payment);
            }
            return result;
        }

        public async Task<ResultModel> Update(Payment payment)
        {
            var result = new ResultModel();
            var validatorResult = new PaymentValidator(_paymentRepository, _creditCardRepository).Validate(payment);
            if (!validatorResult.IsValid)
            {
                result.AddNotification(validatorResult.Errors);
                return result;
            }

            if (result.IsValid)
            {
                if (payment.Monthly)
                    UpdateMonthlyPayment(payment);
                await _paymentRepository.Update(payment);
            }

            return result;
        }

        public async Task<ResultModel> Remove(int paymentId, int userId)
        {
            var result = new ResultModel();
            var payment = await _paymentRepository.GetById(paymentId);
            if (payment is null || payment.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Pagamento"));
            else
                await _paymentRepository.Remove(paymentId);
            return result;
        }

        public async Task UpdateMonthlyPayments(int userId)
        {
            foreach (var payment in (await _paymentRepository.GetSome(new BaseFilter() { UserId = userId })).Where(p => p.Monthly))
                if (FillMonthly(payment))
                    await _paymentRepository.Update(payment);
        }

        private void UpdateMonthlyPayment(Payment payment) => FillMonthly(payment);

        private bool FillMonthly(Payment payment)
        {
            var updated = false;
            var referenceInstallment = payment.Installments.OrderBy(p => p.Date).First();
            var month = referenceInstallment.Date.Month;
            var year = referenceInstallment.Date.Year;
            var now = _paymentRepository.CurrentDate;

            while (year < now.Year || (month <= now.Month && year == now.Year))
            {
                if (!payment.Installments.Any(p => p.Date.Year == year && p.Date.Month == month))
                {
                    payment.Installments.Add(new Installment()
                    {
                        Cost = referenceInstallment.Cost,
                        Date = new DateTime(year, month, referenceInstallment.Date.Day),
                        PaymentId = payment.Id
                    });
                    updated = true;
                }

                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }

            return updated;
        }

        private decimal CalculatePaymentHomeChartModel(IEnumerable<Payment> payments, int month, int year, PaymentTypeEnum type)
        {
            decimal value = 0;
            foreach (var item in payments.Where(p => p.TypeId == type))
                value += item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month).Sum(p => p.Cost);
            return value;
        }
    }
}