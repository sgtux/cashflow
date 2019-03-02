using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
  [TestClass]
  public class AccountTest : BaseTest
  {
    private AccountService _service;

    [TestInitialize]
    public void Init()
    {
      _service = new AccountService(new UserRepositoryMock());
    }

    [TestMethod]
    public void WithInvalidEmail()
    {
      var model = new AccountModel()
      {
        Email = "mstest.mail.com",
        Name = "mstest",
        Password = "1234"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      }, "O email é inválido.");
    }

    [TestMethod]
    public void WithInvalidName()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "",
        Password = "1234"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      }, "O nome é obrigatório.");
    }

    [TestMethod]
    public void WithInvalidPassword()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "mstest",
        Password = "123"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      }, "Informe uma senha de pelo menos 4 dígitos.");
    }

    [TestMethod]
    public void WithEmailAlready()
    {
      var model = new AccountModel()
      {
        Email = "primeirousuario@mail.com",
        Name = "mstest",
        Password = "1234"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      }, "O email informado já está sendo usado.");
    }

    public void WithNameAlready()
    {
      var model = new AccountModel()
      {
        Email = "primeirousuario@mail.com",
        Name = "Primeiro Usuário",
        Password = "1234"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      }, "O nome informado já está sendo usado.");
    }

    public void CreateAccountOk()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "Mstest",
        Password = "1234"
      };
      AssertExceptionMessage(() =>
      {
        _service.Add(model);
      });
    }
  }
}