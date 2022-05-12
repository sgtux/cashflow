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
        private ICreditCardRepository _creditCardRepository;

        public HouseholdExpenseRepository(IDatabaseContext conn,
            LogService logService,
            ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public Task Add(HouseholdExpense t) => Execute(HouseholdExpenseResources.Insert, t);

        public async Task<IEnumerable<HouseholdExpense>> GetSome(BaseFilter filter)
        {
            var list = await Query(HouseholdExpenseResources.Some, filter);
            if (list.Any(p => p.CreditCardId > 0))
            {
                var cards = await _creditCardRepository.GetSome(filter);
                foreach (var item in list)
                    item.CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId);
            }
            return list;
        }

        public async Task<HouseholdExpense> GetById(long id)
        {
            var expense = await FirstOrDefault(HouseholdExpenseResources.ById, new { Id = id });
            if (expense?.CreditCardId > 0)
            {
                var cards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = expense.UserId });
                expense.CreditCard = cards.FirstOrDefault(p => p.Id == expense.CreditCardId);
            }
            return expense;
        }

        public Task Remove(long id) => Execute(HouseholdExpenseResources.Delete, new { Id = id });

        public Task Update(HouseholdExpense t) => Execute(HouseholdExpenseResources.Update, t);
    }
}