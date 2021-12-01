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

namespace Cashflow.Api.Service
{
    public class PaymentService : BaseService
    {
        private IPaymentRepository _paymentRepository;

        private ICreditCardRepository _creditCardRepository;

        private ISalaryRepository _salaryRepository;

        private IDailyExpensesRepository _dailyExpensesRepository;

        private IVehicleRepository _vehicleRepository;

        public PaymentService(IPaymentRepository paymentRepository,
            ICreditCardRepository creditCardRepository,
            ISalaryRepository salaryRepository,
            IDailyExpensesRepository dailyExpensesRepository,
            IVehicleRepository vehicleRepository
            )
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            _salaryRepository = salaryRepository;
            _dailyExpensesRepository = dailyExpensesRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ResultDataModel<Payment>> Get(int id, int userId)
        {
            var p = await _paymentRepository.GetById(id);
            return new ResultDataModel<Payment>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultDataModel<IEnumerable<Payment>>> GetByUser(int userId) => new ResultDataModel<IEnumerable<Payment>>(await _paymentRepository.GetSome(new BaseFilter() { UserId = userId }));

        public async Task<ResultDataModel<IEnumerable<PaymentType>>> GetTypes() => new ResultDataModel<IEnumerable<PaymentType>>(await _paymentRepository.GetTypes());

        public async Task<ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>> GetProjection(int userId, int month, int year)
        {
            var result = new ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>();
            var baseFilter = new BaseFilter() { UserId = userId };
            var payments = (await _paymentRepository.GetSome(baseFilter)).Where(p => p.Active);
            var types = await _paymentRepository.GetTypes();
            var cards = await _creditCardRepository.GetSome(baseFilter);
            var salary = (await _salaryRepository.GetSome(baseFilter)).FirstOrDefault(p => p.EndDate is null);
            var vehicles = await _vehicleRepository.GetSome(baseFilter);
            var allDailyExpenses = await _dailyExpensesRepository.GetSome(baseFilter);

            var dates = LoadDates(month, year);

            dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
            {
                var resultModel = new PaymentProjectionResultModel();
                if (salary != null)
                {
                    resultModel.Payments.Add(new PaymentProjectionModel()
                    {
                        Description = "Salário",
                        Monthly = true,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Gain),
                        Cost = salary.Value
                    });
                }

                FillVehicleExpenses(userId, resultModel, vehicles, date, types);

                FillDailyExpenses(resultModel, allDailyExpenses, date, types);

                foreach (var payMonth in payments.Where(p => p.Monthly || (p.Installments?.Any(p => p.Date.Year == date.Year && p.Date.Month == date.Month) ?? false)))
                {
                    var installment = payMonth.Installments.First(p => payMonth.Monthly || (p.Date.Year == date.Year && p.Date.Month == date.Month));
                    resultModel.Payments.Add(new PaymentProjectionModel()
                    {
                        CreditCard = cards.FirstOrDefault(p => p.Id == payMonth.CreditCardId),
                        Description = payMonth.Description,
                        Monthly = payMonth.Monthly,
                        Condition = payMonth.Condition,
                        MonthYear = date.ToString("MM/yyyy"),
                        Number = installment.Number,
                        PaidDate = installment.PaidDate,
                        QtdInstallments = payMonth.Installments.Count,
                        Type = payMonth.Type,
                        Cost = installment.Cost
                    });
                };

                result.Data.Add(date.ToString("MM/yyyy"), resultModel);
                resultModel.AccumulatedCost = result.Data.Values.Sum(p => p.Total);
            });

            return result;
        }

        public async Task<ResultDataModel<List<HomeChartModel>>> GetHomeChart(int userId, int month, int year)
        {
            var result = new ResultDataModel<List<HomeChartModel>>();
            var filter = new BaseFilter()
            {
                StartDate = new DateTime(year, month, 1),
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                UserId = userId
            };

            var expenses = new HomeChartModel() { Index = 0, Description = "Gastos Essênciais" };
            var dailyExpensesModel = new HomeChartModel() { Index = 1, Description = "Despesas Diárias" };
            var vehicleModel = new HomeChartModel() { Index = 2, Description = "Combustível" };
            var financings = new HomeChartModel() { Index = 3, Description = "Financiamentos" };
            var loan = new HomeChartModel() { Index = 4, Description = "Empréstimos" };
            var contributions = new HomeChartModel() { Index = 5, Description = "Aportes (Investimentos)" };
            var education = new HomeChartModel() { Index = 6, Description = "Educação" };

            foreach (var item in (await _vehicleRepository.GetSome(filter)))
                vehicleModel.Value += item.FuelExpenses.Sum(p => p.ValueSupplied);

            foreach (var item in (await _dailyExpensesRepository.GetSome(filter)))
                dailyExpensesModel.Value += item.TotalPrice;

            var payments = await _paymentRepository.GetSome(filter);

            expenses.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Expense);
            contributions.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Contributions);
            financings.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Financing);
            education.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Education);
            loan.Value += CalculatePaymentHomeChartModel(payments, month, year, PaymentTypeEnum.Loan);

            result.Data.Add(expenses);
            result.Data.Add(dailyExpensesModel);
            result.Data.Add(vehicleModel);
            result.Data.Add(financings);
            result.Data.Add(loan);
            result.Data.Add(contributions);
            result.Data.Add(education);

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
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
            {
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
                result.AddNotification(ValidatorMessages.Payment.NotFound);
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

        private List<DateTime> LoadDates(int month, int year)
        {
            var now = _paymentRepository.CurrentDate;
            var dates = new List<DateTime>();

            var baseDate = new DateTime(year, month, 1);

            if (baseDate == default(DateTime) || baseDate < now)
                baseDate = now.AddMonths(11);
            else
                baseDate.AddMonths(1);

            var currentDate = now;
            var months = 0;
            while (baseDate.Month != currentDate.Month || baseDate.Year != currentDate.Year)
            {
                currentDate = now.AddMonths(months);
                dates.Add(currentDate);
                months++;
            }
            return dates;
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

        private void FillVehicleExpenses(int userId, PaymentProjectionResultModel resultModel, IEnumerable<Vehicle> vehicles, DateTime date, IEnumerable<PaymentType> types)
        {
            foreach (var item in vehicles)
            {
                var fuelExpenses = item.FuelExpenses.Where(p => p.Date.Month == date.Month && p.Date.Year == date.Year);
                if (fuelExpenses.Any())
                {
                    resultModel.Payments.Add(new PaymentProjectionModel()
                    {
                        Description = $"Gastos em Combustível ({item.Description})",
                        Monthly = false,
                        Condition = PaymentConditionEnum.Cash,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                        Cost = fuelExpenses.Sum(p => p.ValueSupplied)
                    });
                }
            }
        }

        private void FillDailyExpenses(PaymentProjectionResultModel resultModel, IEnumerable<DailyExpenses> allDailyExpenses, DateTime date, IEnumerable<PaymentType> types)
        {
            var dailyExpenses = allDailyExpenses.Where(p => p.Date.Month == date.Month && p.Date.Year == date.Year);
            if (dailyExpenses.Any())
            {
                resultModel.Payments.Add(new PaymentProjectionModel()
                {
                    Description = $"Despesas Diárias",
                    Monthly = false,
                    Condition = PaymentConditionEnum.Cash,
                    MonthYear = date.ToString("MM/yyyy"),
                    Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                    Cost = dailyExpenses.Sum(p => p.TotalPrice)
                });
            }
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