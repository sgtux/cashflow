using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface IRecurringExpenseRepository : IRepository<RecurringExpense>
    {

        Task AddHistory(RecurringExpenseHistory history);

        Task UpdateHistory(RecurringExpenseHistory history);

        Task RemoveHistory(long id);
    }
}