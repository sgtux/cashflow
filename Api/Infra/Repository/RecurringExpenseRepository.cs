using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.RecurringExpense;
using Cashflow.Api.Service;

namespace Cashflow.Api.Infra.Repository
{
    public class RecurringExpenseRepository : BaseRepository<RecurringExpense>, IRecurringExpenseRepository
    {
        public RecurringExpenseRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(RecurringExpense t) => Execute(RecurringExpenseResources.Insert, t);

        public Task<IEnumerable<RecurringExpense>> GetSome(BaseFilter filter) => Query(RecurringExpenseResources.Some, filter);

        public Task<RecurringExpense> GetById(long id) => FirstOrDefault(RecurringExpenseResources.ById, new { Id = id });

        public Task Remove(long id) => Execute(RecurringExpenseResources.Delete, new { Id = id });

        public Task Update(RecurringExpense t) => Execute(RecurringExpenseResources.Update, t);

        public Task AddHistory(RecurringExpenseHistory history) => Execute(RecurringExpenseHistoryResources.Insert, history);

        public Task UpdateHistory(RecurringExpenseHistory history) => Execute(RecurringExpenseHistoryResources.Update, history);

        public Task RemoveHistory(long id) => Execute(RecurringExpenseHistoryResources.Delete, new { Id = id });
    }
}