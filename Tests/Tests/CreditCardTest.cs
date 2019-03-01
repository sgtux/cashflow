using Cashflow.Api.Service;
using Cashflow.Tests.Mocks;
using FinanceApi.Infra.Entity;
using FinanceApi.Shared;
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
    public void GetAll()
    {
      var count = _service.GetByUser(1).Count;
      Assert.IsTrue(count > 0);
    }

    [TestMethod]
    public void InsertCardWithNoName()
    {
      AssertExceptionMessage(() =>
      {
        _service.Add("", 1);
      }, "O nome do cartão é obrigatório.");
    }

    [TestMethod]
    public void InsertCardWithUserThatNotExists()
    {
      AssertExceptionMessage(() =>
      {
        _service.Add("nome do cartão", 99);
      }, "Usuário não localizado.");
    }

    [TestMethod]
    public void InsertCardOK()
    {
      AssertExceptionMessage(() =>
      {
        _service.Add("nome do cartão", 2);
      });
    }

    [TestMethod]
    public void UpdateCardWithNoName()
    {
      AssertExceptionMessage(() =>
      {
        _service.Update(new CreditCard() { Name = "", UserId = 1 });
      }, "O nome do cartão é obrigatório.");
    }

    [TestMethod]
    public void UpdateCardWithUserThatNotExists()
    {
      AssertExceptionMessage(() =>
      {
        _service.Update(new CreditCard() { Name = "nome do cartão", UserId = 99 });
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void UpdateCardOK()
    {
      AssertExceptionMessage(() =>
      {
        _service.Update(new CreditCard() { Name = "nome do cartão", UserId = 1, Id = 1 });
      });
    }

    [TestMethod]
    public void RemoveWithNotFound()
    {
      AssertExceptionMessage(() =>
      {
        _service.Remove(99, 2);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void RemoveWithPayments()
    {
      AssertExceptionMessage(() =>
      {
        _service.Remove(1, 1);
      }, "O cartão possui pagamentos vinculados e não pode ser excluído.");
    }

    [TestMethod]
    public void RemoveOK()
    {
      AssertExceptionMessage(() =>
      {
        _service.Remove(2, 1);
      });
    }
  }
}