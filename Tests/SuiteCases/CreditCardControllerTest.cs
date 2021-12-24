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
            Assert.IsTrue(result.Data.Count() == 2);
        }

        [TestMethod]
        public async Task GetByUSer2()
        {
            var result = await Get<IEnumerable<CreditCard>>("/api/CreditCard", 2);
            Assert.IsTrue(result.Data.Count() == 1);
        }

        [TestMethod]
        public async Task AddCardWithNoName()
        {
            var model = new CreditCard() { Name = "", InvoiceDay = 5 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task AddCardWithInvalidInvoiceDay0()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDay = 0 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task InsertCardWithInvalidInvoiceDay31()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDay = 31 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result, "O valor do campo 'Dia da fatura' deve estar entre 1 e 30.");
        }

        [TestMethod]
        public async Task AddCardWithInvoiceDay1OK()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDay = 1 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task AddCardWithInvoiceDay30OK()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDay = 30 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task InsertCardOK()
        {
            var model = new CreditCard() { Name = "nome do cartão", InvoiceDay = 5 };
            var result = await Post("/api/CreditCard", model, 3);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateCardWithNotFound()
        {
            var model = new CreditCard() { Name = "teste", Id = 99, InvoiceDay = 5 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateCardWithNoName()
        {
            var model = new CreditCard() { Name = "", UserId = 1, InvoiceDay = 5 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateCardFromAnotherUser()
        {
            var model = new CreditCard() { Name = "teste", Id = 1, InvoiceDay = 5 };
            var result = await Put("/api/CreditCard", model, 3);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateCardOK()
        {
            var model = new CreditCard() { Name = "nome do cartão", Id = 1, InvoiceDay = 5 };
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