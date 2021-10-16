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
            return await Query<DailyExpensesItem>(DailyExpensesResources.ByUser, (p, i) =>
            {
                if (p.Id == i.DailyExpensesId)
                {
                    if (p.Items == null)
                        p.Items = new List<DailyExpensesItem>();
                    p.Items.Add(i);
                }
                return p;
            }, new { UserId = userId });
        }

        public Task<System.Collections.Generic.IEnumerable<DailyExpenses>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<DailyExpenses> GetById(long id)
        {
            var list = await Query<DailyExpensesItem>(DailyExpensesResources.ById, (p, i) =>
            {
                if (p.Id == i.DailyExpensesId)
                {
                    if (p.Items == null)
                        p.Items = new List<DailyExpensesItem>();
                    p.Items.Add(i);
                }
                return p;
            }, new { Id = id });
            return list.FirstOrDefault();
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