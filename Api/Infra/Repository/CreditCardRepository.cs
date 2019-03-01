using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  /// Credit card repository
  public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
  {

    ///Constructor
    public CreditCardRepository(AppDbContext context) : base(context) { }

    /// Get credit cards of the user
    public List<CreditCard> GetByUserId(int userId) => _context.CreditCard.Where(p => p.UserId == userId).ToList();

    /// Check if credit card has linked payments
    public bool HasPayments(int cardId) => _context.Payment.Any(p => p.CreditCardId == cardId);
  }
}