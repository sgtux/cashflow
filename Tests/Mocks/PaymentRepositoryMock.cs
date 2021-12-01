using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
    public class PaymentRepositoryMock : BaseRepositoryMock, IPaymentRepository
    {

        public Task Add(Payment t) => Task.Run(() => Payments.Add(t));

        public Task<IEnumerable<Payment>> GetAll() => Task.Run(() => Payments.AsEnumerable());

        public Task<Payment> GetById(long id) => Task.Run(() => Payments.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Payment>> GetSome(BaseFilter filter) => Task.Run(() => Payments.Where(p => p.UserId == filter.UserId));

        public Task Remove(long id)
        {
            return Task.Run(() =>
            {
                var payment = Payments.FirstOrDefault(p => p.Id == id);
                if (payment != null)
                    Payments.Remove(payment);
            });
        }

        public Task Update(Payment t) => Task.Run(() => { });

        public Task<bool> Exists(long userId) => Task.Run(() => Payments.Any(p => p.Id == userId));

        public Task<IEnumerable<PaymentType>> GetTypes() => Task.Run(() => PaymentTypes.AsEnumerable());
    }
}