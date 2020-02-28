using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
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
    public async Task WithInvalidEmail()
    {
      var model = new AccountModel()
      {
        Email = "mstest.mail.com",
        Name = "mstest",
        Password = "123456"
      };
      var result = await _service.Add(model.Map(new User()));

      HasNotifications(result, "'Email' is not a valid email address.");
    }

    [TestMethod]
    public async Task WithInvalidName()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "",
        Password = "123456"
      };
      var result = await _service.Add(model.Map(new User()));
      HasNotifications(result, "'Name' must not be empty.");
    }

    [TestMethod]
    public async Task WithInvalidPassword()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "mstest",
        Password = "123"
      };
      var result = await _service.Add(model.Map(new User()));
      HasNotifications(result, "The length of 'Password' must be at least 6 characters. You entered 3 characters.");
    }

    [TestMethod]
    public async Task WithEmailAlready()
    {
      var model = new AccountModel()
      {
        Email = "primeirousuario@mail.com",
        Name = "mstest",
        Password = "123456"
      };
      var result = await _service.Add(model.Map(new User()));
      HasNotifications(result, "The email is already being used.");
    }

    public async Task CreateAccountOk()
    {
      var model = new AccountModel()
      {
        Email = "mstest@mail.com",
        Name = "Mstest",
        Password = "123456"
      };
      var result = await _service.Add(model.Map(new User()));
      HasNotifications(result);
    }
  }
}