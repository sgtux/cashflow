using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
  {
    public CreditCardRepository(DbConfig config) : base(config) { }

    public void Add(CreditCard t)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<CreditCard> GetAll()
    {
      throw new NotImplementedException();
    }

    public CreditCard GetById(int id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<CreditCard> GetByUserId(int userId) => new List<CreditCard>();

    public IEnumerable<CreditCard> GetSome(Expression<Func<CreditCard, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public bool HasPayments(int cardId) => cardId > 0;

    public void Remove(int id)
    {
      throw new NotImplementedException();
    }

    public void Save()
    {
      throw new NotImplementedException();
    }

    public void Update(CreditCard t)
    {
      throw new NotImplementedException();
    }
  }
}