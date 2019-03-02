using System.Collections.Generic;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  /// Payment repository contract
  public interface IPaymentRepository : IRepository<Payment>
  {
    /// Get payments by user id  
    List<Payment> GetByUser(int userId);
  }
}