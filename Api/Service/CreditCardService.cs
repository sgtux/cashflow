using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra.Entity;
using FinanceApi.Infra.Repository;

namespace Cashflow.Api.Service
{
  /// <summary>
  /// Credit card service
  /// </summary>
  public class CreditCardService : BaseService
  {
    private ICreditCardRepository _creditCardRepository;
    private IUserRepository _userRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    public CreditCardService(ICreditCardRepository creditCardRepository, IUserRepository userRepository)
    {
      _creditCardRepository = creditCardRepository;
      _userRepository = userRepository;
    }

    /// <summary>
    /// Get all credit cards of the logged in user
    /// </summary>
    public List<CreditCard> GetByUser(int userId) => _creditCardRepository.GetByUserId(userId);

    /// <summary>
    /// Insert a new credit card for the logged in user
    /// </summary>
    public void Add(string name, int userId)
    {
      if (!_userRepository.UserExists(userId))
        ThrowValidationError("Usuário não localizado.");
      if (string.IsNullOrEmpty(name))
        ThrowValidationError("O nome do cartão é obrigatório.");
      _creditCardRepository.Add(new CreditCard()
      {
        Name = name,
        UserId = userId
      });
      _creditCardRepository.Save();
    }

    /// <summary>
    /// Update credit card
    /// </summary>
    public void Update(CreditCard card)
    {
      if (string.IsNullOrEmpty(card.Name))
        ThrowValidationError("O nome do cartão é obrigatório.");

      var dbCard = _creditCardRepository.GetByUserId(card.UserId).FirstOrDefault(p => p.Id == card.Id);
      if (dbCard is null)
        ThrowValidationError("Cartão não localizado.");

      dbCard.Name = card.Name;
      _creditCardRepository.Update(dbCard);
      _creditCardRepository.Save();
    }

    /// <summary>
    /// Remove a credit card
    /// </summary>
    public void Remove(int id, int userId)
    {
      var card = _creditCardRepository.GetByUserId(userId).FirstOrDefault(p => p.Id == id);
      if (card is null)
        ThrowValidationError("Cartão não localizado.");

      if (_creditCardRepository.HasPayments(id))
        ThrowValidationError("O cartão possui pagamentos vinculados e não pode ser excluído.");

      _creditCardRepository.Remove(id);
      _creditCardRepository.Save();
    }
  }
}