using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Enums;
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
                Description = filterModel.Description.FormatToLike(),
                StartDate = filterModel.StartDate.FixStartTimeFilter(),
                EndDate = filterModel.StartDate.FixEndTimeFilter()
            };
            var list = await _paymentRepository.GetSome(filter);
            if (filterModel.Done.HasValue)
            {
                if (filterModel.Done.Value)
                    list = list.Where(p => p.Done);
                else
                    list = list.Where(p => !p.Done);
            }
            list = list.OrderByDescending(p => p.Date);
            return new ResultDataModel<IEnumerable<Payment>>(list);
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

            var householdExpenseModel = new HomeChartModel() { Index = 0, Description = "Despesas Domésticas" };
            var expensesModel = new HomeChartModel() { Index = 1, Description = "Outros Gastos" };
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
            {
                result.AddNotification(validatorResult.Errors);
                return result;
            }

            await _paymentRepository.Add(payment);

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

            await _paymentRepository.Update(payment);

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

        public async Task<ResultModel> GenerateInstallments(GenerateInstallmentsModel model, int userId)
        {
            var installments = new List<Installment>();
            var result = new ResultDataModel<List<Installment>>(installments);

            if (model.Value <= 0 || model.Amount <= 0 || model.Amount > 72 || model.Date == default(DateTime))
            {
                if (model.Value <= 0)
                    result.AddNotification("Valor deve ser maior que 0.");

                if (model.Amount <= 0 || model.Amount > 72)
                    result.AddNotification("Quantidade de parcelas deve estar entre 1 e 72.");

                if (model.Date == default(DateTime))
                    result.AddNotification("Data inválida.");

                return result;
            }

            var day = model.Date.Day;
            var month = model.Date.Month;
            var year = model.Date.Year;

            if (model.CreditCardId > 0)
            {
                var creditCard = await _creditCardRepository.GetById(model.CreditCardId);

                if (creditCard == null || creditCard.UserId != userId)
                {
                    result.AddNotification(ValidatorMessages.NotFound("Cartão de Crédito"));
                    return result;
                }

                if (day > creditCard.InvoiceClosingDay)
                {
                    month++;
                    if (month > 12)
                    {
                        month = 1;
                        year++;
                    }
                }
                day = creditCard.InvoiceDueDay;
            }

            var firstValue = model.Value;

            if (!model.ValueByInstallment)
            {
                var total = model.Value;
                model.Value = Math.Round(total / model.Amount, 2, MidpointRounding.ToZero);
                var sum = Math.Round(model.Value * model.Amount, 2);
                firstValue = model.Value + (total > sum ? total - sum : sum - total);
            }

            for (short i = 1; i <= model.Amount; i++)
            {
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
                installments.Add(new Installment()
                {
                    Number = i,
                    Value = model.Value,
                    Date = new DateTime(year, month, day)
                });
                month++;
            }

            installments.First().Value = firstValue;

            return result;
        }

        private decimal CalculatePaymentHomeChartModel(IEnumerable<Payment> payments, int month, int year, PaymentTypeEnum type)
        {
            decimal value = 0;
            foreach (var item in payments.Where(p => p.TypeId == type))
                value += item.Installments.Where(p => p.Date.Year == year && p.Date.Month == month).Sum(p => p.Value);
            return value;
        }
    }
}