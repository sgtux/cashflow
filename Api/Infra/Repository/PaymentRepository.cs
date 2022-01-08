using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.Payment;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private ICreditCardRepository _creditCardRepository;

        public PaymentRepository(IDatabaseContext conn, LogService logService, ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public async Task<IEnumerable<PaymentType>> GetTypes()
        {
            var list = new List<Payment>();
            return await Query<PaymentType>(PaymentResources.Types);
        }

        public async Task<IEnumerable<Payment>> GetSome(BaseFilter filter)
        {
            var list = new List<Payment>();
            var types = await GetTypes();
            var cards = await _creditCardRepository.GetSome(filter);
            await Query<Installment>(PaymentResources.Some, (p, i) =>
            {
                var pay = list.FirstOrDefault(x => x.Id == p.Id);
                if (pay == null)
                {
                    pay = p;
                    if (p.CreditCardId.HasValue)
                        p.CreditCard = cards.FirstOrDefault(c => c.Id == p.CreditCardId.Value);
                    list.Add(p);
                    pay.Type = types.FirstOrDefault(t => t.Id == (int)p.TypeId);
                    pay.Installments = new List<Installment>();
                }
                pay.Installments.Add(i);
                return p;
            }, filter);
            return list.OrderBy(p => p.Description);
        }

        public async Task<Payment> GetById(long id)
        {
            Payment payment = null;
            var types = await GetTypes();
            await Query<Installment>(PaymentResources.ById, (p, i) =>
            {
                if (payment == null)
                {
                    payment = p;
                    payment.Installments = new List<Installment>();
                    payment.Type = types.FirstOrDefault(t => t.Id == (int)p.TypeId);
                }
                payment.Installments.Add(i);
                return p;
            }, new { Id = id });

            if (payment != null)
            {
                var cards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = payment.UserId });
                if (payment.CreditCardId.HasValue)
                    payment.CreditCard = cards.FirstOrDefault(c => c.Id == payment.CreditCardId.Value);
            }
            return payment;
        }

        public async Task Add(Payment payment)
        {
            BeginTransaction();
            await Execute(PaymentResources.Insert, payment);
            var currentId = await NextId();
            foreach (var i in payment.Installments)
            {
                i.PaymentId = currentId;
                await Execute(InstallmentResources.Insert, i);
            }
        }

        public async Task Update(Payment payment)
        {
            BeginTransaction();
            await Execute(InstallmentResources.Delete, new { PaymentId = payment.Id });
            await Execute(PaymentResources.Update, payment);
            foreach (var i in payment.Installments.OrderBy(p => p.Number))
            {
                i.PaymentId = payment.Id;
                await Execute(InstallmentResources.Insert, i);
            }
        }

        public async Task Remove(long id)
        {
            BeginTransaction();
            await Execute(InstallmentResources.Delete, new { PaymentId = id });
            await Execute(PaymentResources.Delete, new { Id = id });
        }

        public DateTime CurrentDate => DateTime.Now;
    }
}