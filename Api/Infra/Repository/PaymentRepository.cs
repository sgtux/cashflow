using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    public PaymentRepository(AppConfig config) : base(config) { }

    public IEnumerable<Payment> GetByUser(int userId)
    {
      return new List<Payment>();
    }

    public Payment GetById(int id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Payment> GetAll()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Payment> GetSome(Expression<Func<Payment, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public void Add(Payment t)
    {
      throw new NotImplementedException();
    }

    public void Update(Payment t)
    {
      throw new NotImplementedException();
    }

    public void Remove(int id)
    {
      throw new NotImplementedException();
    }

    public void Save()
    {
      throw new NotImplementedException();
    }

    public DateTime CurrentDate => DateTime.Now;
  }
}