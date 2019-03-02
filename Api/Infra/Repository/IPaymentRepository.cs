using System.Collections.Generic;
using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  /// Payment repository contract
  public interface IPaymentRepository : IRepository<Payment>
  {
    /// Get payments by user id  
    List<Payment> GetByUser(int userId);
  }
}