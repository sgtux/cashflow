using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Tests.Mocks
{
    public class DailyExpensesRepositoryMock : BaseRepositoryMock, IDailyExpensesRepository
    {
        public Task Add(DailyExpenses t) => Task.Run(() => DailyExpenses.Add(t));

        public Task<IEnumerable<DailyExpenses>> GetAll() => Task.Run(() => DailyExpenses.AsEnumerable());

        public Task<DailyExpenses> GetById(long id) => Task.Run(() => DailyExpenses.FirstOrDefault(p => p.Id == id));

        public Task Remove(long id) => Task.Run(() => DailyExpenses.Remove(DailyExpenses.FirstOrDefault(p => p.Id == id)));

        public Task Update(DailyExpenses t)
        {
            return Task.Run(() =>
            {
                var dailyExpenses = DailyExpenses.FirstOrDefault(p => p.Id == t.Id);
                dailyExpenses.Date = t.Date;
                dailyExpenses.Items = t.Items;
                dailyExpenses.ShopName = t.ShopName;
            });
        }

        public Task<bool> Exists(long id) => Task.Run(() => DailyExpenses.Any(p => p.Id == id));

        public Task<IEnumerable<DailyExpenses>> GetSome(BaseFilter filter) => Task.Run(() => DailyExpenses.Where(p => p.UserId == filter.UserId));
    }
}