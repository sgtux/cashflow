using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
  {
    public CreditCardRepository(AppConfig config) : base(config) { }

    public Task Add(CreditCard t)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<CreditCard>> GetAll()
    {
      throw new NotImplementedException();
    }

    public Task<CreditCard> GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<CreditCard>> GetByUserId(int userId) => throw new NotImplementedException();

    public Task<IEnumerable<CreditCard>> GetSome(Expression<Func<CreditCard, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public Task<bool> HasPayments(int cardId) => throw new NotImplementedException();

    public Task Remove(int id)
    {
      throw new NotImplementedException();
    }

    public Task Update(CreditCard t)
    {
      throw new NotImplementedException();
    }
  }
}