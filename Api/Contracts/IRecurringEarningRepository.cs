using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRecurringEarningRepository : IRepository<RecurringEarningEntity, RecurringEarningFilter>
    {
        Task AddHistory(RecurringEarningHistoryEntity history);

        Task UpdateHistory(RecurringEarningHistoryEntity history);

        Task RemoveHistory(long id);
    }
}
