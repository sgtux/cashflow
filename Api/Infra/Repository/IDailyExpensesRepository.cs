using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
    public interface IDailyExpensesRepository : IRepository<DailyExpenses>
    {
        Task<IEnumerable<DailyExpenses>> GetByUser(int userId);
    }
}