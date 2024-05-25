using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Models;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class AccountTest : BaseControllerTest
    {
        [TestMethod]
        public async Task AddWithInvalidEmail()
        {
            var model = new AccountModel()
            {
                Email = "",
                Password = "12345678"
            };

            var result = await Post<AccountResultModel>("/api/account", model);

            Trace.WriteLine(JsonSerializer.Serialize(result));

            TestErrors(model, result, "O campo 'Email' deve ter pelo menos 6 caracteres.");
        }

        [TestMethod]
        public async Task AddWithInvalidPassword()
        {
            var model = new AccountModel()
            {
                Email = "mstest",
                Password = "123"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result, "O campo 'Senha' deve ter pelo menos 8 caracteres.");
        }

        [TestMethod]
        public async Task AddWithNickNameAlreadyUsed()
        {
            var model = new AccountModel()
            {
                Email = "User1@mail.com",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result, "O Email informado já está sendo utilizado.");
        }

        [TestMethod]
        public async Task AddWithNickNameOutOfPattern()
        {
            var model = new AccountModel()
            {
                Email = "Primeiro Usuario",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result, "Deve ser informado um Email válido.");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = new AccountModel()
            {
                Email = "Mstest@mail.com",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task GetDataOk()
        {
            var result = await Get<UserDataModel>("/api/account", 3);
            Assert.AreEqual("User3@mail.com", result.Data.Email);
        }
    }
}