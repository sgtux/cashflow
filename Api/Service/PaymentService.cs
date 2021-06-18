using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class PaymentService : BaseService
    {
        private IPaymentRepository _paymentRepository;
        private ICreditCardRepository _creditCardRepository;

        public PaymentService(IPaymentRepository paymentRepository, ICreditCardRepository creditCardRepository)
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<ResultModel> GetByUser(int userId) => new ResultDataModel<IEnumerable<Payment>>(await _paymentRepository.GetByUser(userId));

        public async Task<Dictionary<string, PaymentFutureResultModel>> GetFuturePayments(int userId, DateTime forecastAt)
        {
            var result = new Dictionary<string, PaymentFutureResultModel>();
            var now = _paymentRepository.CurrentDate;
            var dates = new List<DateTime>();

            if (forecastAt == default(DateTime) || forecastAt < now)
                forecastAt = now.AddMonths(11);
            else
                forecastAt.AddMonths(1);

            var currentDate = now;
            var months = 0;
            while (forecastAt.Month != currentDate.Month || forecastAt.Year != currentDate.Year)
            {
                currentDate = now.AddMonths(months);
                dates.Add(currentDate);
                months++;
            }

            var payments = await _paymentRepository.GetByUser(userId);
            var cards = await _creditCardRepository.GetByUserId(userId);

            dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
            {
                var resultModel = new PaymentFutureResultModel();

                foreach (var payMonth in payments.Where(p => p.FixedPayment || (p.Installments?.Any(p => p.Date.Year == date.Year && p.Date.Month == date.Month) ?? false)))
                {
                    var installment = payMonth.Installments.First(p => payMonth.FixedPayment || (p.Date.Year == date.Year && p.Date.Month == date.Month));
                    resultModel.Payments.Add(new PaymentFutureModel()
                    {
                        CreditCard = cards.FirstOrDefault(p => p.Id == payMonth.CreditCardId),
                        Description = payMonth.Description,
                        FixedPayment = payMonth.FixedPayment,
                        Invoice = payMonth.Invoice,
                        MonthYear = date.ToString("MM/yyyy"),
                        Number = installment.Number,
                        Paid = installment.Paid,
                        QtdInstallments = payMonth.Installments.Count,
                        Type = payMonth.Type,
                        Cost = installment.Cost
                    });
                };

                result.Add(date.ToString("MM/yyyy"), resultModel);
                resultModel.AccumulatedCost = result.Values.Sum(p => p.CostIncome) - result.Values.Sum(p => p.CostExpense);
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
                await _paymentRepository.Add(payment);
            return result;
        }

        public async Task<ResultModel> Update(Payment payment)
        {
            var result = new ResultModel();
            var validatorResult = new PaymentValidator(_paymentRepository, _creditCardRepository).Validate(payment);
            if (!validatorResult.IsValid)
                result.AddNotification(validatorResult.Errors);

            if (result.IsValid)
                await _paymentRepository.Update(payment);
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
    }
}