using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.CreditCard;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(CreditCard card) => Execute(CreditCardResources.Insert, card);

        public Task<CreditCard> GetById(long id) => throw new NotImplementedException();

        public Task<IEnumerable<CreditCard>> GetSome(BaseFilter filter) => Query(CreditCardResources.ByUser, filter);

        public async Task<bool> HasPayments(int cardId)
        {
            return await ExecuteScalar<int>(CreditCardResources.HasPayments, new { Id = cardId }) > 0;
        }

        public Task Remove(long id) => Execute(CreditCardResources.Delete, new { Id = id });

        public Task Update(CreditCard card) => Execute(CreditCardResources.Update, card);
    }
}