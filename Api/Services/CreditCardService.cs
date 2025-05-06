using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
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

        public async Task<ResultDataModel<IEnumerable<CreditCard>>> GetByUser(int userId)
        {
            var creditCards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = userId });

            var creditCardIds = creditCards.Select(p => p.Id);

            var payments = (await _paymentService.GetByUser(userId, new PaymentFilter() { Done = false, CreditCardIds = creditCardIds })).Data;
            var householdExpenses = (await _householdExpenseService.GetByUser(userId, 0, 0, creditCardIds)).Data;
            var recurringExpenses = (await _recurringExpenseService.GetByUser(userId, 1, creditCardIds)).Data;

            foreach (var card in creditCards)
            {
                foreach (var pay in payments.Where(p => p.CreditCardId == card.Id && p.HasInstallments))
                {
                    var item = new CreditCardItemModel();
                    item.Description = $"{pay.Description} (Parcelado)";
                    item.OutstandingDebt = pay.Total - pay.TotalPaid;
                    item.Plots = $"{pay.Installments.Count(p => p.PaidValue.HasValue)}/{pay.Installments.Count}";
                    item.Total = pay.Total;
                    card.Items.Add(item);
                }

                foreach (var householdExpense in householdExpenses.Where(p => p.CreditCardId == card.Id))
                {
                    var item = new CreditCardItemModel();
                    item.Description = $"{householdExpense.Description} (Despesa)";
                    item.OutstandingDebt = householdExpense.Value;
                    card.Items.Add(item);
                }

                foreach (var recurringExpense in recurringExpenses.Where(p => p.CreditCardId == card.Id))
                {
                    var item = new CreditCardItemModel();
                    item.Description = $"{recurringExpense.Description} (Despesa Recorrente)";
                    item.OutstandingDebt = recurringExpense.Value;
                    card.Items.Add(item);
                }
            }

            return new ResultDataModel<IEnumerable<CreditCard>>(creditCards);
        }

        public async Task<ResultModel> Add(CreditCard card)
        {
            var result = new ResultModel();
            var validatorResult = new CreditCardValidator(_creditCardRepository, _userRepository).Validate(card);

            if (validatorResult.IsValid)
                await _creditCardRepository.Add(card);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(CreditCard card)
        {
            var result = new ResultModel();
            var validatorResult = new CreditCardValidator(_creditCardRepository, _userRepository).Validate(card);

            if (validatorResult.IsValid)
                await _creditCardRepository.Update(card);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();
            var card = (await _creditCardRepository.GetSome(new BaseFilter() { UserId = userId })).FirstOrDefault(p => p.Id == id);
            if (card is null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Cartão de Crédito"));
                return result;
            }

            var hasPayments = await _creditCardRepository.HasPayments(id);
            if (hasPayments)
                result.AddNotification(ValidatorMessages.CreditCard.BindedWithPayments);

            var hasHouseholdExpenses = await _creditCardRepository.HasHouseholdExpenses(id);
            if (hasHouseholdExpenses)
                result.AddNotification(ValidatorMessages.CreditCard.BindedHouseholdExpense);

            if (!result.IsValid)
                return result;

            await _creditCardRepository.Remove(id);
            return result;
        }
    }
}