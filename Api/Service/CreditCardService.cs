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
      var validatorResult = new CreditCardValidator().Validate(card);
      if (!validatorResult.IsValid)
      {
        result.AddNotification(validatorResult.Errors);
        return result;
      }

      if (!await _userRepository.Exists(card.UserId))
      {
        result.AddNotification("User not found.");
        return result;
      }

      await _creditCardRepository.Add(card);
      return result;
    }

    public async Task<ResultModel> Update(CreditCard card)
    {
      var result = new ResultModel();
      var validatorResult = new CreditCardValidator().Validate(card);
      if (!validatorResult.IsValid)
      {
        result.AddNotification(validatorResult.Errors);
        return result;
      }

      var dbCard = (await _creditCardRepository.GetByUserId(card.UserId)).FirstOrDefault(p => p.Id == card.Id);
      if (dbCard is null)
      {
        result.AddNotification("Credit card not found.");
        return result;
      }

      dbCard.Name = card.Name;
      await _creditCardRepository.Update(dbCard);
      return result;
    }

    public async Task<ResultModel> Remove(int id, int userId)
    {
      var result = new ResultModel();
      var card = (await _creditCardRepository.GetByUserId(userId)).FirstOrDefault(p => p.Id == id);
      if (card is null)
      {
        result.AddNotification("Credit card not found.");
        return result;
      }

      if (_creditCardRepository.HasPayments(id).Result)
      {
        result.AddNotification("The card has linked payments and can't be deleted.");
        return result;
      }

      await _creditCardRepository.Remove(id);
      return result;
    }
  }
}