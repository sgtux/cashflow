using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.HouseholdExpense;
using Cashflow.Api.Services;

namespace Cashflow.Api.Infra.Repository
{
    public class HouseholdExpenseRepository : BaseRepository<HouseholdExpenseEntity>, IHouseholdExpenseRepository
    {
        public HouseholdExpenseRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(HouseholdExpenseEntity t) => Execute(HouseholdExpenseResources.Insert, t);

        public async Task<IEnumerable<HouseholdExpenseEntity>> GetSome(HouseholdExpenseFilter filter)
        {
            var data = await Query<dynamic>(HouseholdExpenseResources.Some, filter);
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(HouseholdExpenseEntity), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCardEntity), new List<string> { "Id" });
            return Slapper.AutoMapper.MapDynamic<HouseholdExpenseEntity>(data);
        }

        public async Task<HouseholdExpenseEntity> GetById(long id)
        {
            var data = await Query<dynamic>(HouseholdExpenseResources.ById, new { Id = id });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(HouseholdExpenseEntity), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCardEntity), new List<string> { "Id" });
            return Slapper.AutoMapper.MapDynamic<HouseholdExpenseEntity>(data).FirstOrDefault();
        }

        public Task Remove(long id) => Execute(HouseholdExpenseResources.Delete, new { Id = id });

        public Task Update(HouseholdExpenseEntity t) => Execute(HouseholdExpenseResources.Update, t);
    }
}