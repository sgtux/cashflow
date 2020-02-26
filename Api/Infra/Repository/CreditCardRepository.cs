using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Infra.Resources.CreditCard;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
  {
    public CreditCardRepository(AppConfig config) : base(config) { }

    public Task Add(CreditCard card) => Execute(CreditCardResources.Insert, card);

    public Task<IEnumerable<CreditCard>> GetAll() => throw new NotImplementedException();

    public Task<CreditCard> GetById(int id) => throw new NotImplementedException();

    public Task<IEnumerable<CreditCard>> GetByUserId(int userId) => Query(CreditCardResources.ByUser, new { UserId = userId });

    public Task<IEnumerable<CreditCard>> GetSome(Expression<Func<CreditCard, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> HasPayments(int cardId)
    {
      return await ExecuteScalar<int>(CreditCardResources.HasPayments, new { Id = cardId }) > 0;
    }

    public Task Remove(int id) => Execute(CreditCardResources.Delete, new { Id = id });

    public Task Update(CreditCard card) =>  Execute(CreditCardResources.Update, card);
  }
}