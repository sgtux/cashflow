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
        public PaymentRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public async Task<IEnumerable<Payment>> GetSome(PaymentFilter filter)
        {
            var data = await Query<dynamic>(PaymentResources.Some, filter);

            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Payment), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Installment), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCard), new List<string> { "Id" });

            return Slapper.AutoMapper.MapDynamic<Payment>(data);
        }

        public async Task<Payment> GetById(long id)
        {
            var data = await Query<dynamic>(PaymentResources.ById, new { Id = id });

            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Payment), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Installment), new List<string> { "Id" });
            Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(CreditCard), new List<string> { "Id" });

            return Slapper.AutoMapper.MapDynamic<Payment>(data).FirstOrDefault();
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
    }
}