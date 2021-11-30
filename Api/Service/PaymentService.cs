using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
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

        public async Task<ResultDataModel<IEnumerable<Payment>>> GetByUser(int userId) => new ResultDataModel<IEnumerable<Payment>>(await _paymentRepository.GetByUser(userId));

        public async Task<ResultDataModel<IEnumerable<PaymentType>>> GetTypes() => new ResultDataModel<IEnumerable<PaymentType>>(await _paymentRepository.GetTypes());

        public async Task<ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>> GetProjection(int userId, int month, int year)
        {
            var result = new ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>();
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

            var payments = (await _paymentRepository.GetByUser(userId)).Where(p => p.Active);
            var types = await _paymentRepository.GetTypes();
            var cards = await _creditCardRepository.GetByUserId(userId);
            var salary = (await _salaryRepository.GetByUserId(userId)).FirstOrDefault(p => p.EndDate is null);
            var vehicles = await _vehicleRepository.GetByUserId(userId);
            var allDailyExpenses = await _dailyExpensesRepository.GetByUser(userId);

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
            foreach (var payment in (await _paymentRepository.GetByUser(userId)).Where(p => p.Monthly))
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
    }
}