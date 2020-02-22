using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    public PaymentRepository(AppConfig config) : base(config) { }

    public Task<IEnumerable<Payment>> GetByUser(int userId)
    {
      throw new NotImplementedException();
    }

    public Task<Payment> GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Payment>> GetAll()
    {
      throw new NotImplementedException();
    }

    public Task Add(Payment t)
    {
      throw new NotImplementedException();
    }

    public Task Update(Payment t)
    {
      throw new NotImplementedException();
    }

    public Task Remove(int id)
    {
      throw new NotImplementedException();
    }

    public DateTime CurrentDate => DateTime.Now;
  }
}