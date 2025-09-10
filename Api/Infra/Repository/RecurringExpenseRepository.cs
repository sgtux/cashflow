using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.RecurringExpense;
using Cashflow.Api.Services;

namespace Cashflow.Api.Infra.Repository
{
    public class RecurringExpenseRepository : BaseRepository<RecurringExpenseEntity>, IRecurringExpenseRepository
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public RecurringExpenseRepository(IDatabaseContext conn, LogService logService, ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public async Task<IEnumerable<RecurringExpenseEntity>> GetSome(RecurringExpenseFilter filter)
        {
            var list = new List<RecurringExpenseEntity>();
            var cards = await _creditCardRepository.GetSome(filter);
            await Query<RecurringExpenseHistoryEntity>(RecurringExpenseResources.Some, (p, i) =>
            {
                var recurringExpense = list.FirstOrDefault(x => x.Id == p.Id);
                if (recurringExpense == null)
                {
                    recurringExpense = p;
                    list.Add(recurringExpense);
                    if (recurringExpense.CreditCardId.HasValue)
                        recurringExpense.CreditCard = cards.FirstOrDefault(p => p.Id == recurringExpense.CreditCardId.Value);
                    recurringExpense.History = new List<RecurringExpenseHistoryEntity>();
                }
                if (i != null)
                    recurringExpense.History.Add(i);
                return p;
            }, filter);
            list.ForEach(p => p.SortHistory());
            return list;
        }

        public async Task<RecurringExpenseEntity> GetById(long id)
        {
            RecurringExpenseEntity recurringExpense = null;
            await Query<RecurringExpenseHistoryEntity>(RecurringExpenseResources.ById, (p, i) =>
            {
                if (recurringExpense == null)
                {
                    recurringExpense = p;
                    recurringExpense.History = new List<RecurringExpenseHistoryEntity>();
                }
                if (i != null)
                    recurringExpense.History.Add(i);
                return p;
            }, new { Id = id });

            if (recurringExpense != null)
            {
                recurringExpense.SortHistory();
                if (recurringExpense.CreditCardId.HasValue)
                {
                    var cards = await _creditCardRepository.GetSome(new BaseFilter { UserId = recurringExpense.UserId });
                    recurringExpense.CreditCard = cards.FirstOrDefault(p => p.Id == recurringExpense.CreditCardId.Value);
                }
            }

            return recurringExpense;
        }

        public Task Add(RecurringExpenseEntity t) => Execute(RecurringExpenseResources.Insert, t);

        public Task Remove(long id) => Execute(RecurringExpenseResources.Delete, new { Id = id });

        public Task Update(RecurringExpenseEntity t) => Execute(RecurringExpenseResources.Update, t);

        public Task AddHistory(RecurringExpenseHistoryEntity history) => Execute(RecurringExpenseHistoryResources.Insert, history);

        public Task UpdateHistory(RecurringExpenseHistoryEntity history) => Execute(RecurringExpenseHistoryResources.Update, history);

        public Task RemoveHistory(long id) => Execute(RecurringExpenseHistoryResources.Delete, new { Id = id });
    }
}