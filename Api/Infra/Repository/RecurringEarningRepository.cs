using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.RecurringEarning;
using Cashflow.Api.Services;

namespace Cashflow.Api.Infra.Repository
{
    public class RecurringEarningRepository : BaseRepository<RecurringEarningEntity>, IRecurringEarningRepository
    {
        public RecurringEarningRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public async Task<IEnumerable<RecurringEarningEntity>> GetSome(RecurringEarningFilter filter)
        {
            var list = new List<RecurringEarningEntity>();
            await Query<RecurringEarningHistoryEntity>(RecurringEarningResources.Some, (p, i) =>
            {
                var recurringEarning = list.FirstOrDefault(x => x.Id == p.Id);
                if (recurringEarning == null)
                {
                    recurringEarning = p;
                    list.Add(recurringEarning);
                    recurringEarning.History = new List<RecurringEarningHistoryEntity>();
                }
                if (i != null)
                    recurringEarning.History.Add(i);
                return p;
            }, filter);
            list.ForEach(p => p.SortHistory());
            return list;
        }

        public async Task<RecurringEarningEntity> GetById(long id)
        {
            RecurringEarningEntity recurringEarning = null;
            await Query<RecurringEarningHistoryEntity>(RecurringEarningResources.ById, (p, i) =>
            {
                if (recurringEarning == null)
                {
                    recurringEarning = p;
                    recurringEarning.History = new List<RecurringEarningHistoryEntity>();
                }
                if (i != null)
                    recurringEarning.History.Add(i);
                return p;
            }, new { Id = id });

            recurringEarning?.SortHistory();
            return recurringEarning;
        }

        public Task Add(RecurringEarningEntity t) => Execute(RecurringEarningResources.Insert, t);

        public Task Update(RecurringEarningEntity t) => Execute(RecurringEarningResources.Update, t);

        public Task Remove(long id) => Execute(RecurringEarningResources.Inactivate, new { Id = id, InactiveAt = CurrentDate });

        public Task AddHistory(RecurringEarningHistoryEntity history) => Execute(RecurringEarningHistoryResources.Insert, history);

        public Task UpdateHistory(RecurringEarningHistoryEntity history) => Execute(RecurringEarningHistoryResources.Update, history);

        public Task RemoveHistory(long id) => Execute(RecurringEarningHistoryResources.Delete, new { Id = id });
    }
}
