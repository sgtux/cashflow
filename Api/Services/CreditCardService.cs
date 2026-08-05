using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Models.CreditCard;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class CreditCardService : BaseService
    {
        private readonly ICreditCardRepository _creditCardRepository;

        private readonly IUserRepository _userRepository;

        private readonly PaymentService _paymentService;

        private readonly HouseholdExpenseService _householdExpenseService;

        private readonly RecurringExpenseService _recurringExpenseService;

        public CreditCardService(
            ICreditCardRepository creditCardRepository,
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            IRecurringExpenseRepository recurringExpenseRepository,
            IHouseholdExpenseRepository householdExpenseRepository,
            IVehicleRepository vehicleRepository,
            AppCache appCache)
        {
            _creditCardRepository = creditCardRepository;
            _userRepository = userRepository;
            _paymentService = new PaymentService(paymentRepository, _creditCardRepository, appCache);
            _householdExpenseService = new HouseholdExpenseService(householdExpenseRepository, vehicleRepository, appCache, creditCardRepository);
            _recurringExpenseService = new RecurringExpenseService(recurringExpenseRepository, creditCardRepository, appCache);
        }

        public async Task<ResultDataModel<IEnumerable<CreditCardEntity>>> GetByUser(int userId)
        {
            var now = CurrentDate;
            var creditCards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = userId });

            var creditCardIds = creditCards.Select(p => p.Id);

            var payments = (await _paymentService.GetByUser(userId, new PaymentFilter() { Done = false, CreditCardIds = creditCardIds })).Data;
            var householdExpenses = (await _householdExpenseService.GetByUser(userId, now.AddMonths(-1), null, creditCardIds)).Data;
            var recurringExpenses = (await _recurringExpenseService.GetByUser(userId, 1, creditCardIds)).Data;

            foreach (var card in creditCards)
            {
                foreach (var pay in payments.Where(p => p.CreditCardId == card.Id && p.HasInstallments))
                {
                    var item = new CreditCardItemModel();
                    item.Id = pay.Id;
                    item.Type = CreditCardExpenseType.Installment;
                    item.Description = $"{pay.Description} (Parcelado)";
                    item.OutstandingDebt = pay.Total - pay.TotalPaid;
                    item.Plots = $"{pay.Installments.Count(p => p.PaidValue.HasValue)}/{pay.Installments.Count}";
                    item.Total = pay.Total;
                    var currentInstallment = pay.Installments.FirstOrDefault(p => p.Date.SameMonthYear(now));
                    if (currentInstallment is not null)
                    {
                        item.IsCurrentMonthDebtPaid = currentInstallment.PaidDate is not null;
                        item.CurrentMonthDebt = currentInstallment.PaidValue ?? currentInstallment.Value;
                    }
                    card.Items.Add(item);
                }

                foreach (var householdExpense in householdExpenses.Where(p => p.CreditCardId == card.Id && (p.InvoiceDate.SameMonthYear(now) || p.InvoiceDate.SameMonthYear(now.AddMonths(1)))))
                {
                    var item = new CreditCardItemModel();
                    item.Id = householdExpense.Id;
                    item.Type = CreditCardExpenseType.Household;
                    item.Description = $"{householdExpense.Description} (Despesa)";
                    item.OutstandingDebt = householdExpense.Value;
                    item.IsCurrentMonthDebtPaid = householdExpense.InvoiceDate.SameMonthYear(now);
                    item.CurrentMonthDebt = householdExpense.Value;
                    card.Items.Add(item);
                }

                foreach (var recurringExpense in recurringExpenses.Where(p => p.CreditCardId == card.Id))
                {
                    var item = new CreditCardItemModel();
                    item.Id = recurringExpense.Id;
                    item.Type = CreditCardExpenseType.Recurring;
                    item.Description = $"{recurringExpense.Description} (Despesa Recorrente)";
                    item.OutstandingDebt = recurringExpense.Value;
                    var currentHistory = recurringExpense.History.FirstOrDefault(p => p.Date.SameMonthYear(now));
                    if (currentHistory is not null)
                    {
                        item.IsCurrentMonthDebtPaid = true;
                        item.CurrentMonthDebt = currentHistory.PaidValue;
                    }
                    else
                        item.CurrentMonthDebt = recurringExpense.Value;
                    card.Items.Add(item);
                }
            }

            return new ResultDataModel<IEnumerable<CreditCardEntity>>(creditCards);
        }

        public async Task<ResultModel> Add(CreditCardEntity card)
        {
            var result = new ResultModel();
            var validatorResult = await new CreditCardValidator(_creditCardRepository, _userRepository).ValidateAsync(card);

            if (validatorResult.IsValid)
                await _creditCardRepository.Add(card);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(CreditCardEntity card)
        {
            var result = new ResultModel();
            var validatorResult = await new CreditCardValidator(_creditCardRepository, _userRepository).ValidateAsync(card);

            if (validatorResult.IsValid)
                await _creditCardRepository.Update(card);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();
            var card = await _creditCardRepository.GetById(id);
            if (card is null || card.UserId != userId)
            {
                result.AddNotification(ValidatorMessages.NotFound("Cartão de Crédito"));
                return result;
            }

            if (await _creditCardRepository.HasPayments(id))
                result.AddNotification(ValidatorMessages.CreditCard.BindedWithPayments);

            if (await _creditCardRepository.HasHouseholdExpenses(id))
                result.AddNotification(ValidatorMessages.CreditCard.BindedHouseholdExpense);

            if (!result.IsValid)
                return result;

            await _creditCardRepository.Remove(id);
            return result;
        }

        public async Task<ResultModel> PayCurrentInvoicePayment(PayCurrentInvoicePaymentModel model, int userId)
        {
            var result = new ResultModel();

            if (model.PaidValue <= 0)
            {
                result.AddNotification(ValidatorMessages.CreditCard.InvalidPaymentValue);
                return result;
            }

            var card = await _creditCardRepository.GetById(model.CreditCardId);
            if (card is null || card.UserId != userId)
            {
                result.AddNotification(ValidatorMessages.NotFound("Cartão de Crédito"));
                return result;
            }

            DateTime paidDate = new(CurrentDate.Year, CurrentDate.Month, card.InvoiceDueDay);

            switch (model.Type)
            {
                case CreditCardExpenseType.Installment:
                    await PayCurrentInstallment(model, userId, result, paidDate);
                    break;
                case CreditCardExpenseType.Household:
                    break;
                case CreditCardExpenseType.Recurring:
                    return await PayCurrentRecurring(model, userId, paidDate);
                default:
                    result.AddNotification(ValidatorMessages.CreditCard.InvalidType);
                    return result;
            }

            return result;
        }

        private async Task PayCurrentInstallment(PayCurrentInvoicePaymentModel model, int userId, ResultModel result, DateTime paidDate)
        {
            var payment = (await _paymentService.Get(model.ItemId, userId)).Data;
            if (payment is null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Pagamento"));
                return;
            }

            var installment = payment.Installments.Where(p => p.Date.SameMonthYear(CurrentDate)).FirstOrDefault();
            if (installment is null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Parcela Mês Atual"));
                return;
            }

            installment.PaidValue = model.PaidValue;
            installment.PaidDate = paidDate;

            await _paymentService.UpdateInstallment(installment);
        }

        private Task<ResultModel> PayCurrentRecurring(PayCurrentInvoicePaymentModel model, int userId, DateTime paidDate)
        {
            var history = new RecurringExpenseHistoryEntity()
            {
                RecurringExpenseId = model.ItemId,
                Date = paidDate,
                PaidValue = model.PaidValue
            };

            return _recurringExpenseService.AddHistory(history, userId);
        }
    }
}