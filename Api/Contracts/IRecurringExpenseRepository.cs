using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRecurringExpenseRepository : IRepository<RecurringExpenseEntity, RecurringExpenseFilter>
    {
        Task AddHistory(RecurringExpenseHistoryEntity history);

        Task UpdateHistory(RecurringExpenseHistoryEntity history);

        Task RemoveHistory(long id);
    }
}