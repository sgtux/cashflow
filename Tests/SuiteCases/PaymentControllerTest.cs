using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class PaymentControllerTest : BaseControllerTest
    {
        private Payment DefaultPayment
        => new Payment()
        {
            Id = 1,
            UserId = 1,
            CreditCardId = 1,
            BaseCost = 20.5M,
            Description = "First Payment",
            Condition = PaymentConditionEnum.Cash,
            Type = new PaymentType() { Id = (int)PaymentTypeEnum.Expense },
            TypeId = PaymentTypeEnum.Expense,
            Installments = new List<Installment>()
                    {
                      new Installment() { Number = 1, Id = 1, Cost = 1500.6M, Date = new DateTime(2020, 1, 1) }
                    }
        };

        [TestMethod]
        public async Task GetUserPayments()
        {
            var payments = await Get<IEnumerable<Payment>>("/api/Payment", 1);
            Assert.IsFalse(payments.Data.Any(p => p.UserId != 1));
        }

        [TestMethod]
        public async Task AddWithNoDescription()
        {
            var p = DefaultPayment;
            p.Description = "";
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithNoPlots()
        {
            var p = DefaultPayment;
            p.Installments = new List<Installment>();
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "O pagamento deve ter pelo menos 1 parcela.");
        }

        [TestMethod]
        public async Task AddWithCreditCardIdNotFound()
        {
            var p = DefaultPayment;
            p.CreditCardId = 99;
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task AddInvalidBaseValue()
        {
            var p = DefaultPayment;
            p.Id = 0;
            p.BaseCost = 0;
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "O campo 'Valor Base' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddOK()
        {
            var p = DefaultPayment;
            p.CreditCardId = 1;
            p.Id = 0;
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result);
        }

        [TestMethod]
        public async Task UpdateWithNoDescription()
        {
            var p = DefaultPayment;
            p.Description = "";
            var result = await Put("/api/Payment", p, p.UserId);
            TestErrors(p, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateWithNoPlots()
        {
            var p = DefaultPayment;
            p.Installments = new List<Installment>();
            var result = await Put("/api/Payment", p, p.UserId);
            TestErrors(p, result, "O pagamento deve ter pelo menos 1 parcela.");
        }

        [TestMethod]
        public async Task UpdatePaymentNotFound()
        {
            var p = DefaultPayment;
            p.Id = 99;
            p.CreditCardId = 1;
            var result = await Put("/api/Payment", p, p.UserId);
            TestErrors(p, result, "Pagamento não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateWithCreditCardIdNotFound()
        {
            var p = DefaultPayment;
            p.CreditCardId = 99;
            var result = await Put("/api/Payment", p, p.UserId);
            TestErrors(p, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateOK()
        {
            var p = DefaultPayment;
            p.CreditCardId = 1;
            var result = await Put("/api/Payment", p, p.UserId);
            TestErrors(p, result);
        }

        [TestMethod]
        public async Task RemoveWithPaymentNotFound()
        {
            var result = await Delete("/api/Payment/99", 1);
            TestErrors(new { a = 99, b = 1 }, result, "Pagamento não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveOK()
        {
            var result = await Delete("/api/Payment/3", 1);
            TestErrors(new { a = 3, b = 1 }, result);
        }

        [TestMethod]
        public async Task GetPaymentTypesOK()
        {
            var result = await Get<IEnumerable<PaymentType>>("/api/Payment/Types", 1);
            Assert.IsTrue(result.Data.Any());
        }
    }
}