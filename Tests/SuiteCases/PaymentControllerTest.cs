using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums = Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cashflow.Api.Models;
using Cashflow.Api.Enums;

namespace Cashflow.Tests
{
    [TestClass]
    [TestCategory("PaymentTest")]
    public class PaymentControllerTest : BaseControllerTest
    {
        private Payment DefaultPayment
        => new Payment()
        {
            Id = 1,
            UserId = 1,
            CreditCardId = 1,
            Description = "First Payment",
            Type = ExpenseType.Others,
            Installments = new List<Installment>()
                    {
                      new Installment() { Number = 1, Id = 1, Value = 1500.6M, PaidValue = 1500.6M, Date = new DateTime(2020, 1, 1) },
                      new Installment() { Number = 2, Id = 1, Value = 1500.6M, Exempt = true, Date = new DateTime(2020, 1, 1) }
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
        public async Task AddInstallmentExemptWithPaidValue()
        {
            var p = DefaultPayment;
            p.Installments = new List<Installment>()
                {
                    new Installment() { Number = 1, Id = 1, Exempt = true, PaidValue = 1500.6M, PaidDate = DateTime.Now, Value = 1500.6M, Date = new DateTime(2020, 1, 1) }
                };
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "Parcela isenta com valor pago informado.");
        }

        [TestMethod]
        public async Task AddNoExemptInstallmentWithNoPaidValue()
        {
            var p = DefaultPayment;
            p.Installments = new List<Installment>()
                {
                    new Installment() { Number = 1, Id = 1, Value = 1500.6M, PaidValue = 0, Date = new DateTime(2020, 1, 1) }
                };
            var result = await Post("/api/Payment", p, p.UserId);
            TestErrors(p, result, "Parcela com valor pago inválido.");
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
            var result = await Get<IEnumerable<TypeModel>>("/api/Payment/Types", 1);
            Assert.IsTrue(result.Data.Any());
        }

        [TestMethod]
        public async Task GetGenerateInstallmentsOK()
        {
            string queryString = "value=1000&amount=10&date=2024-06-10T03:00:00.000Z";
            var result = await Get<IEnumerable<Installment>>($"/api/Payment/GenerateInstallments?{queryString}", 1);
            var first = result.Data.First();
            Assert.AreEqual(100, first.Value);
            Assert.AreEqual(6, first.Date.Month);
        }

        [TestMethod]
        public async Task GetGenerateInstallmentsWithCreditCardOK()
        {
            // CreditCard 4 with UserId 1, InvoiceDueDay 20, InvoiceClosingDay = 10
            string queryString = "value=1000&amount=10&date=2024-06-09T03:00:00.000Z&creditCardId=4";
            var result = await Get<IEnumerable<Installment>>($"/api/Payment/GenerateInstallments?{queryString}", 1);
            var first = result.Data.First();
            Assert.AreEqual(100, first.Value);
            Assert.AreEqual(6, first.Date.Month);
        }

        [TestMethod]
        public async Task GetGenerateInstallmentsWithCreditCardNextMonthOK()
        {
            // CreditCard 5 with UserId 1, InvoiceDueDay 10, InvoiceClosingDay = 20
            string queryString = "value=1000&amount=10&date=2024-06-10T03:00:00.000Z&creditCardId=5";
            var result = await Get<IEnumerable<Installment>>($"/api/Payment/GenerateInstallments?{queryString}", 1);
            var first = result.Data.First();
            Assert.AreEqual(100, first.Value);
            Assert.AreEqual(7, first.Date.Month);
        }

        [TestMethod]
        public async Task GetGenerateInstallmentsWithCreditCardNextNextMonthOK()
        {
            // CreditCard 5 with UserId 1, InvoiceDueDay 10, InvoiceClosingDay = 20
            string queryString = "value=1000&amount=10&date=2024-06-20T03:00:00.000Z&creditCardId=5";
            var result = await Get<IEnumerable<Installment>>($"/api/Payment/GenerateInstallments?{queryString}", 1);
            var first = result.Data.First();
            Assert.AreEqual(100, first.Value);
            Assert.AreEqual(8, first.Date.Month);
        }
    }
}