using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
  {
    public CreditCardRepository(AppDbContext context) : base(context) { }
  }
}