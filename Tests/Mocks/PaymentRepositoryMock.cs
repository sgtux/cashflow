using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
  public class PaymentRepositoryMock : BaseRepositoryMock, IPaymentRepository
  {
    public void Add(Payment t) => Payments.Add(t);

    public List<Payment> GetAll() => Payments;

    public Payment GetById(int id) => Payments.FirstOrDefault(p => p.Id == id);

    public List<Payment> GetByUser(int userId) => Payments.Where(p => p.UserId == userId).ToList();

    public List<Payment> GetSome(Expression<Func<Payment, bool>> expressions) => throw new NotImplementedException();

    public void Remove(int id)
    {
      var payment = Payments.FirstOrDefault(p => p.Id == id);
      if (payment != null)
        Payments.Remove(payment);
    }

    public void Save() { }

    public void Update(Payment t)
    {
      
    }
  }
}