using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;
using Cashflow.Api.Extensions;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Services
{
    public class PaymentService : BaseService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

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

        public ResultDataModel<IEnumerable<TypeModel>> GetTypes()
        {
            var types = Enum.GetValues<PaymentType>().Select(p => new TypeModel(p));
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
    }
}