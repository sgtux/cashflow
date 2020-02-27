using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Infra.Resources.Payment;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    public PaymentRepository(AppConfig config) : base(config) { }

    public async Task<IEnumerable<Payment>> GetByUser(int userId)
    {
      var list = new List<Payment>();
      var result = await Query<Installment>(PaymentResources.ByUser, (p, i) =>
      {
        var pay = list.FirstOrDefault(x => x.Id == p.Id);
        if (pay == null)
        {
          pay = p;
          list.Add(p);
          pay.Installments = new List<Installment>();
        }
        pay.Installments.Add(i);
        return p;
      }, new { UserId = userId });
      return list;
    }

    public Task<Payment> GetById(int id) => FirstOrDefault(PaymentResources.ById, new { Id = id });

    public Task<IEnumerable<Payment>> GetAll()
    {
      throw new NotImplementedException();
    }

    public async Task Add(Payment payment)
    {
      await Execute(PaymentResources.Insert, payment);
      foreach (var i in payment.Installments)
      {
        i.PaymentId = payment.Id;
        await Execute(InstallmentResources.Insert, i);
      }
    }

    public async Task Update(Payment payment)
    {
      await Execute(InstallmentResources.Delete, new { PaymentId = payment.Id });
      await Execute(PaymentResources.Update, payment);
      int number = 0;
      foreach (var i in payment.Installments.OrderBy(p => p.Number))
      {
        i.Number = number;
        i.PaymentId = payment.Id;
        await Execute(InstallmentResources.Insert, i);
      }
    }

    public Task Remove(int id) => Execute(PaymentResources.Delete, new { Id = id });

    public DateTime CurrentDate => DateTime.Now;
  }
}