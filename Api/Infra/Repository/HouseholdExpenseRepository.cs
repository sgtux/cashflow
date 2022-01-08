using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.HouseholdExpense;
using Cashflow.Api.Services;

namespace Cashflow.Api.Infra.Repository
{
    public class HouseholdExpenseRepository : BaseRepository<HouseholdExpense>, IHouseholdExpenseRepository
    {
        public HouseholdExpenseRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(HouseholdExpense t) => Execute(HouseholdExpenseResources.Insert, t);

        public Task<IEnumerable<HouseholdExpense>> GetSome(BaseFilter filter) => Query(HouseholdExpenseResources.Some, filter);

        public Task<HouseholdExpense> GetById(long id) => FirstOrDefault(HouseholdExpenseResources.ById, new { Id = id });

        public Task Remove(long id) => Execute(HouseholdExpenseResources.Delete, new { Id = id });

        public Task Update(HouseholdExpense t) => Execute(HouseholdExpenseResources.Update, t);
    }
}