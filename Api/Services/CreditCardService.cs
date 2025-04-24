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

        public CreditCardService(
            ICreditCardRepository creditCardRepository,
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            AppCache appCache)
        {
            _creditCardRepository = creditCardRepository;
            _userRepository = userRepository;
            _paymentService = new PaymentService(paymentRepository, _creditCardRepository, appCache);
        }

        public async Task<ResultDataModel<IEnumerable<CreditCard>>> GetByUser(int userId)
        {
            var payments = (await _paymentService.GetByUser(userId, new PaymentFilterModel() { Done = false })).Data;

            var creditCards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = userId });

            foreach (var card in creditCards)
            {
                foreach (var pay in payments.Where(p => p.CreditCard?.Id == card.Id && p.HasInstallments))
                {
                    card.OutstandingDebt += pay.Installments.Where(p => !p.Exempt && !p.PaidValue.HasValue).Sum(p => p.Value);
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