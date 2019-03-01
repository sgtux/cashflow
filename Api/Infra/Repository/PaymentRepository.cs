using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  /// Payment repository
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    /// Constructor
    public PaymentRepository(AppDbContext context) : base(context) { }
  }
}