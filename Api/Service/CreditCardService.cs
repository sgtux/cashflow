using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class CreditCardService : BaseService
    {
        private ICreditCardRepository _creditCardRepository;

        private IUserRepository _userRepository;

        public CreditCardService(ICreditCardRepository creditCardRepository, IUserRepository userRepository)
        {
            _creditCardRepository = creditCardRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<CreditCard>> GetByUser(int userId) => await _creditCardRepository.GetByUserId(userId);

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
            var card = (await _creditCardRepository.GetByUserId(userId)).FirstOrDefault(p => p.Id == id);
            if (card is null)
            {
                result.AddNotification(ValidatorMessages.CreditCard.NotFound);
                return result;
            }

            if (_creditCardRepository.HasPayments(id).Result)
            {
                result.AddNotification(ValidatorMessages.CreditCard.BindedWithPayments);
                return result;
            }

            await _creditCardRepository.Remove(id);
            return result;
        }
    }
}