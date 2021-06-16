using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.Infra.Resources.Payment;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(DatabaseContext conn) : base(conn) { }

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

        public async Task<Payment> GetById(int id)
        {
            Payment payment = null;
            var result = await Query<Installment>(PaymentResources.ById, (p, i) =>
            {
                if (payment == null)
                {
                    payment = p;
                    payment.Installments = new List<Installment>();
                }
                payment.Installments.Add(i);
                return p;
            }, new { Id = id });
            return payment;
        }

        public Task<IEnumerable<Payment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task Add(Payment payment)
        {
            BeginTransaction();
            try
            {
                await Execute(PaymentResources.Insert, payment);
                var currentId = await NextId();
                foreach (var i in payment.Installments)
                {
                    i.PaymentId = currentId;
                    await Execute(InstallmentResources.Insert, i);
                }
                Commit();
            }
            catch (Exception) { Rollback(); }
        }

        public async Task Update(Payment payment)
        {
            BeginTransaction();
            try
            {
                var payDb = await GetById(payment.Id);
                if (payDb == null)
                    throw new Exception("Payment not found");
                await Execute(InstallmentResources.Delete, new { PaymentId = payment.Id });
                await Execute(PaymentResources.Update, payment);
                int number = 0;
                foreach (var i in payment.Installments.OrderBy(p => p.Number))
                {
                    i.Number = number;
                    i.PaymentId = payment.Id;
                    await Execute(InstallmentResources.Insert, i);
                }
                payDb = await GetById(payment.Id);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public async Task Remove(int id)
        {
            await Execute(InstallmentResources.Delete, new { PaymentId = id });
            await Execute(PaymentResources.Delete, new { Id = id });
        }

        public DateTime CurrentDate => DateTime.Now;
    }
}