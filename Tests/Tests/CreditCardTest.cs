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
      Assert.IsTrue(result.Count() > 0);
    }

    [TestMethod]
    public async Task InsertCardWithNoName()
    {
      var result = await _service.Add(new CreditCard() { Name = "", UserId = 1 });
      HasNotifications(result, "'Name' must not be empty.");
    }

    [TestMethod]
    public async Task InsertCardWithUserThatNotExists()
    {
      var result = await _service.Add(new CreditCard() { Name = "nome do cartão", UserId = 999 });
      HasNotifications(result, "User not found.");
    }

    [TestMethod]
    public async Task InsertCardOK()
    {
      var result = await _service.Add(new CreditCard() { Name = "nome do cartão", UserId = 2 });
      HasNotifications(result);
    }

    [TestMethod]
    public async Task UpdateCardWithNoName()
    {
      var result = await _service.Update(new CreditCard() { Name = "", UserId = 1 });
      HasNotifications(result, "'Name' must not be empty.");
    }

    [TestMethod]
    public async Task UpdateCardWithUserThatNotExists()
    {
      var result = await _service.Update(new CreditCard() { Name = "nome do cartão", UserId = 99 });
      HasNotifications(result, "Credit card not found.");
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
      HasNotifications(result, "Credit card not found.");
    }

    [TestMethod]
    public async Task RemoveWithPayments()
    {
      var result = await _service.Remove(1, 1);
      HasNotifications(result, "The card has linked payments and can't be deleted.");
    }

    [TestMethod]
    public async Task RemoveOK()
    {
      var result = await _service.Remove(2, 1);
      HasNotifications(result);
    }
  }
}