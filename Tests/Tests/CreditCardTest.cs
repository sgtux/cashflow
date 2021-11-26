using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class CreditCardTest : BaseTest
    {
        private CreditCardService _service;

        [TestInitialize]
        public void Init()
        {
            _service = new CreditCardService(new CreditCardRepositoryMock(), new UserRepositoryMock());
        }

        [TestMethod]
        public async Task GetAll()
        {
            var result = await _service.GetByUser(1);
            Assert.IsTrue(result.Data.Count() > 0);
        }

        [TestMethod]
        public async Task InsertCardWithNoName()
        {
            var result = await _service.Add(new CreditCard() { Name = "", UserId = 1 });
            HasNotifications(result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task InsertCardWithUserThatNotExists()
        {
            var result = await _service.Add(new CreditCard() { Name = "nome do cartão", UserId = 999 });
            HasNotifications(result, "Usuário não encontrado.");
        }

        [TestMethod]
        public async Task InsertCardOK()
        {
            var result = await _service.Add(new CreditCard() { Name = "nome do cartão", UserId = 2 });
            HasNotifications(result);
        }

        [TestMethod]
        public async Task UpdateCardWithNotFound()
        {
            var result = await _service.Update(new CreditCard() { Name = "teste", UserId = 1, Id = 99 });
            HasNotifications(result, "Cartão de crédito não encontrado.");
        }

        [TestMethod]
        public async Task UpdateCardWithNoName()
        {
            var result = await _service.Update(new CreditCard() { Name = "", UserId = 1 });
            HasNotifications(result, "O campo 'Nome' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateCardWithUserThatNotExists()
        {
            var result = await _service.Update(new CreditCard() { Name = "nome do cartão", UserId = 99 });
            HasNotifications(result, "Usuário não encontrado.");
        }

        [TestMethod]
        public async Task UpdateCardOK()
        {
            var result = await _service.Update(new CreditCard() { Name = "nome do cartão", UserId = 1, Id = 1 });
            HasNotifications(result);
        }

        [TestMethod]
        public async Task RemoveWithNotFound()
        {
            var result = await _service.Remove(99, 2);
            HasNotifications(result, "Cartão de crédito não encontrado.");
        }

        [TestMethod]
        public async Task RemoveWithPayments()
        {
            var result = await _service.Remove(1, 1);
            HasNotifications(result, "Este cartão está vinculado à algum pagamento e não pode ser removido.");
        }

        [TestMethod]
        public async Task RemoveOK()
        {
            var result = await _service.Remove(2, 1);
            HasNotifications(result);
        }
    }
}