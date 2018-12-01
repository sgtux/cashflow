using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    public PaymentRepository(AppDbContext context) : base(context) { }
  }
}