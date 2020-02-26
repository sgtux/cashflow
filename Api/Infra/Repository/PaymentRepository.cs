using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Infra.Resources.Payment;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    public PaymentRepository(AppConfig config) : base(config) { }

    public Task<IEnumerable<Payment>> GetByUser(int userId)
    {
      return Query(PaymentResources.ByUser, new { UserId = userId });
    }

    public Task<Payment> GetById(int id) => FirstOrDefault(PaymentResources.ById, new { Id = id });

    public Task<IEnumerable<Payment>> GetAll()
    {
      throw new NotImplementedException();
    }

    public Task Add(Payment payment) => Execute(PaymentResources.Insert, payment);

    public Task Update(Payment payment) => Execute(PaymentResources.Update, payment);

    public Task Remove(int id) => Execute(PaymentResources.Delete, new { Id = id });

    public DateTime CurrentDate => DateTime.Now;
  }
}