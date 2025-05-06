using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface ICreditCardRepository : IRepository<CreditCard, BaseFilter>
    {
        Task<bool> HasPayments(int cardId);

        Task<bool> HasHouseholdExpenses(int cardId);
    }
}