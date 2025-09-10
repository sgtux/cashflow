using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.CreditCard;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class CreditCardRepository : BaseRepository<CreditCardEntity>, ICreditCardRepository
    {
        public CreditCardRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(CreditCardEntity card) => Execute(CreditCardResources.Insert, card);

        public Task<CreditCardEntity> GetById(long id) => FirstOrDefault(CreditCardResources.ById, new { Id = id });

        public Task<IEnumerable<CreditCardEntity>> GetSome(BaseFilter filter) => Query(CreditCardResources.ByUser, filter);

        public async Task<bool> HasPayments(int cardId) => await ExecuteScalar<int>(CreditCardResources.HasPayments, new { Id = cardId }) > 0;

        public async Task<bool> HasHouseholdExpenses(int cardId) => await ExecuteScalar<int>(CreditCardResources.HasHouseholdExpenses, new { Id = cardId }) > 0;

        public Task Remove(long id) => Execute(CreditCardResources.Delete, new { Id = id });

        public Task Update(CreditCardEntity card) => Execute(CreditCardResources.Update, card);
    }
}