using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<PaymentType>> GetTypes();

        System.DateTime CurrentDate { get; }
    }
}