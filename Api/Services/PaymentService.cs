using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;
using Cashflow.Api.Enums;
using Cashflow.Api.Shared.Cache;

namespace Cashflow.Api.Services
{
    public class PaymentService : BaseService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        private readonly AppCache _appCache;

        public PaymentService(IPaymentRepository paymentRepository, ICreditCardRepository creditCardRepository, AppCache appCache)
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<Payment>> Get(int id, int userId)
        {
            var p = await _paymentRepository.GetById(id);
            return new ResultDataModel<Payment>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultDataModel<IEnumerable<Payment>>> GetByUser(int userId, PaymentFilter filter)
        {
            filter.UserId = userId;
            filter.FixParams();
            var list = await _paymentRepository.GetSome(filter);
            if (filter.Done.HasValue)
            {
                if (filter.Done.Value)
                    list = list.Where(p => p.Done);
                else
                    list = list.Where(p => !p.Done);
            }
            list = list.OrderBy(p => p.Description);
            return new ResultDataModel<IEnumerable<Payment>>(list);
        }

        public ResultDataModel<IEnumerable<TypeModel>> GetTypes()
        {
            var types = Enum.GetValues<ExpenseType>()
                            .Where(p => p != ExpenseType.Others)
                            .Select(p => new TypeModel(p))
                            .OrderBy(p => p.Description)
                            .Append(new TypeModel(ExpenseType.Others));
            return new ResultDataModel<IEnumerable<TypeModel>>(types);
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

            if (payment.Date == default)
                payment.Date = payment.Installments.First().Date;

            await _paymentRepository.Add(payment);
            _appCache.Clear(payment.UserId);

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

            if (payment.Date == default)
                payment.Date = payment.Installments.First().Date;

            await _paymentRepository.Update(payment);
            _appCache.Clear(payment.UserId);

            return result;
        }

        public async Task<ResultModel> Remove(int paymentId, int userId)
        {
            var result = new ResultModel();
            var payment = await _paymentRepository.GetById(paymentId);
            if (payment is null || payment.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Pagamento"));
            else
            {
                await _paymentRepository.Remove(paymentId);
                _appCache.Clear(payment.UserId);
            }
            return result;
        }

        public async Task<ResultModel> GenerateInstallments(GenerateInstallmentsModel model, int userId)
        {
            var installments = new List<Installment>();
            var result = new ResultDataModel<List<Installment>>(installments);

            if (model.Value <= 0 || model.Amount <= 0 || model.Amount > 72 || model.Date == default)
            {
                if (model.Value <= 0)
                    result.AddNotification("Valor deve ser maior que 0.");

                if (model.Amount <= 0 || model.Amount > 72)
                    result.AddNotification("Quantidade de parcelas deve estar entre 1 e 72.");

                if (model.Date == default)
                    result.AddNotification("Data inválida.");

                return result;
            }

            var purchaseDate = model.Date;
            var paymentDate = model.Date;

            if (model.CreditCardId > 0)
            {
                var creditCard = await _creditCardRepository.GetById(model.CreditCardId);

                if (creditCard == null || creditCard.UserId != userId)
                {
                    result.AddNotification(ValidatorMessages.NotFound("Cartão de Crédito"));
                    return result;
                }

                if (purchaseDate.Day >= creditCard.InvoiceClosingDay)
                {
                    paymentDate = paymentDate.AddMonths(creditCard.InvoiceClosingDay > creditCard.InvoiceDueDay ? 2 : 1);
                }
                else if (creditCard.InvoiceClosingDay > creditCard.InvoiceDueDay)
                {
                    paymentDate = paymentDate.AddMonths(1);
                }

                paymentDate = new DateTime(paymentDate.Year, paymentDate.Month, creditCard.InvoiceDueDay);
            }

            var total = model.Value;
            model.Value = Math.Round(total / model.Amount, 2, MidpointRounding.ToZero);
            var sum = Math.Round(model.Value * model.Amount, 2);
            var firstValue = model.Value + (total > sum ? total - sum : sum - total);

            for (short i = 1; i <= model.Amount; i++)
            {
                installments.Add(new Installment()
                {
                    Number = i,
                    Value = model.Value,
                    Date = paymentDate
                });
                paymentDate = paymentDate.AddMonths(1);
            }

            installments.First().Value = firstValue;

            return result;
        }
    }
}