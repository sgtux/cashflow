using System.Collections.Generic;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  /// Credit card repository contract
  public interface ICreditCardRepository : IRepository<CreditCard>
  {
    /// Contract to obtain credit cards per user
    List<CreditCard> GetByUserId(int userId);

    /// Check if credit card has linked payments
    bool HasPayments(int cardId);
  }
}