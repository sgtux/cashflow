using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface ICreditCardRepository : IRepository<CreditCard>
    {
        Task<bool> HasPayments(int cardId);
    }
}