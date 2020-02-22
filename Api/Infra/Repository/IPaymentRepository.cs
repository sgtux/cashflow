using System.Collections.Generic;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  public interface IPaymentRepository : IRepository<Payment>
  {
    IEnumerable<Payment> GetByUser(int userId);

    System.DateTime CurrentDate { get; }
  }
}