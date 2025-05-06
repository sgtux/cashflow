using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRecurringExpenseRepository : IRepository<RecurringExpense, RecurringExpenseFilter>
    {
        Task AddHistory(RecurringExpenseHistory history);

        Task UpdateHistory(RecurringExpenseHistory history);

        Task RemoveHistory(long id);
    }
}