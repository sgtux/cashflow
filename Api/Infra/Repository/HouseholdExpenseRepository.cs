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
    public class HouseholdExpenseRepository : BaseRepository<HouseholdExpense>, IHouseholdExpenseRepository
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public HouseholdExpenseRepository(IDatabaseContext conn,
            LogService logService,
            ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public Task Add(HouseholdExpense t) => Execute(HouseholdExpenseResources.Insert, t);

        public async Task<IEnumerable<HouseholdExpense>> GetSome(HouseholdExpenseFilter filter)
        {
            var data = await Query<dynamic>(HouseholdExpenseResources.Some, filter);
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(HouseholdExpense), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCard), new List<string> { "Id" });
            return Slapper.AutoMapper.MapDynamic<HouseholdExpense>(data);
        }

        public async Task<HouseholdExpense> GetById(long id)
        {
            var data = await Query<dynamic>(HouseholdExpenseResources.ById, new { Id = id });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(HouseholdExpense), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCard), new List<string> { "Id" });
            return Slapper.AutoMapper.MapDynamic<HouseholdExpense>(data).FirstOrDefault();
        }

        public Task Remove(long id) => Execute(HouseholdExpenseResources.Delete, new { Id = id });

        public Task Update(HouseholdExpense t) => Execute(HouseholdExpenseResources.Update, t);
    }
}