using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
    public interface ICreditCardRepository : IRepository<CreditCard>
    {
        Task<IEnumerable<CreditCard>> GetByUserId(int userId);

        Task<bool> HasPayments(int cardId);
    }
}