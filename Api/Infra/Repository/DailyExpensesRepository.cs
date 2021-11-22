using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Sql.DailyExpenses;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class DailyExpensesRepository : BaseRepository<DailyExpenses>, IDailyExpensesRepository
    {
        public DailyExpensesRepository(DatabaseContext conn, LogService logService) : base(conn, logService) { }

        public async Task Add(DailyExpenses t)
        {
            BeginTransaction();
            await Execute(DailyExpensesResources.Insert, t);
            var currentId = await NextId();
            foreach (var i in t.Items)
            {
                i.DailyExpensesId = currentId;
                await Execute(DailyExpensesResources.InsertItem, i);
            }
        }

        public async Task<IEnumerable<DailyExpenses>> GetByUser(int userId)
        {
            var list = new List<DailyExpenses>();
            await Query<DailyExpensesItem>(DailyExpensesResources.ByUser, (p, i) =>
            {
                var expense = list.FirstOrDefault(x => x.Id == p.Id);
                if (expense == null)
                {
                    expense = p;
                    p.Items = new List<DailyExpensesItem>();
                    list.Add(p);
                }

                if (i.DailyExpensesId == expense.Id)
                    expense.Items.Add(i);

                return p;
            }, new { UserId = userId });

            return list;
        }

        public async Task<DailyExpenses> GetById(long id)
        {
            DailyExpenses dailyExpenses = null;
            var list = await Query<DailyExpensesItem>(DailyExpensesResources.ById, (p, i) =>
            {
                if (dailyExpenses == null)
                {
                    dailyExpenses = p;
                    dailyExpenses.Items = new List<DailyExpensesItem>();
                }

                if (i.DailyExpensesId == dailyExpenses.Id)
                    dailyExpenses.Items.Add(i);

                return p;
            }, new { Id = id });
            return dailyExpenses;
        }

        public async Task Remove(long id)
        {
            BeginTransaction();
            await Execute(DailyExpensesResources.DeleteItems, new { DailyExpensesId = id });
            await Execute(DailyExpensesResources.Delete, new { Id = id });
        }

        public async Task Update(DailyExpenses t)
        {
            BeginTransaction();
            await Execute(DailyExpensesResources.DeleteItems, new { DailyExpensesId = t.Id });
            await Execute(DailyExpensesResources.Update, t);
            foreach (var i in t.Items)
            {
                i.DailyExpensesId = t.Id;
                await Execute(DailyExpensesResources.InsertItem, i);
            }
        }
    }
}