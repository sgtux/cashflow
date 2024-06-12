using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class CreditCardControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task GetByUSer1()
        {
            var result = await Get<IEnumerable<CreditCard>>("/api/CreditCard", 1);
            Assert.AreEqual(result.Data.Count(), 4, "Quantidade de cartões");
        }

        [TestMethod]
        public async Task GetByUSer2()
        {
            var result = await Get<IEnumerable<CreditCard>>("/api/CreditCard", 2);
            Assert.AreEqual(result.Data.Count(), 1, "Quantidade de cartões");
        }

        [TestMethod]
        public async Task AddCardWithNoName()
        {
            var model = new CreditCard() { Name = "", InvoiceClosingDay = 5, InvoiceDueDay = 10 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task AddCardWithInvalidInvoiceDueDay0()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceClosingDay = 5, InvoiceDueDay = 0 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia de vencimento da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task AddCardWithInvalidInvoiceClosingDay0()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDueDay = 5, InvoiceClosingDay = 0 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia de fechamento da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task InsertCardWithInvalidInvoiceDueDay31()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceClosingDay = 5, InvoiceDueDay = 31 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia de vencimento da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task InsertCardWithInvalidInvoiceClosingDay31()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceClosingDay = 31, InvoiceDueDay = 5 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia de fechamento da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task AddCardWithInvoiceDueDay1OK()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceClosingDay = 1, InvoiceDueDay = 1 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task AddCardWithInvoiceDueDay30OK()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceClosingDay = 30, InvoiceDueDay = 30 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateCardWithNotFound()
        {
            var model = new CreditCard() { Name = "teste", Id = 99 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateCardWithNoName()
        {
            var model = new CreditCard() { Name = "", UserId = 1, InvoiceDueDay = 5, InvoiceClosingDay = 5 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateCardFromAnotherUser()
        {
            var model = new CreditCard() { Name = "teste", Id = 1, InvoiceDueDay = 5, InvoiceClosingDay = 5 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateCardOK()
        {
            var model = new CreditCard() { Name = "nome do cartão", Id = 1, InvoiceDueDay = 5, InvoiceClosingDay = 5 };
            var result = await Put("/api/CreditCard", model, 1);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveWithNotFound()
        {
            var result = await Delete("/api/CreditCard/99", 3);
            TestErrors(new { a = 99, b = 3 }, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveWithPayments()
        {
            var result = await Delete("/api/CreditCard/1", 1);
            TestErrors(new { a = 1, b = 1 }, result, "Este cartão está vinculado à algum pagamento e não pode ser removido.");
        }

        [TestMethod]
        public async Task RemoveOK()
        {
            var result = await Delete("/api/CreditCard/2", 1);
            TestErrors(new { a = 2, b = 1 }, result);
        }
    }
}