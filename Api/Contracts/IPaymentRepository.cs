using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IPaymentRepository : IRepository<Payment, PaymentFilter> { }
}