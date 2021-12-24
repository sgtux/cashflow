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
        public async Task AddWithInvalidNickName()
        {
            var model = new AccountModel()
            {
                NickName = "",
                Password = "12345678"
            };

            var result = await Post<AccountResultModel>("/api/account", model);

            Trace.WriteLine(JsonSerializer.Serialize(result));

            TestErrors(model, result, "O campo 'Nick Name' deve ter pelo menos 4 caracteres.");
        }

        [TestMethod]
        public async Task AddWithInvalidPassword()
        {
            var model = new AccountModel()
            {
                NickName = "mstest",
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
                NickName = "User1",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result, "O Nick Name informado já está sendo utilizado.");
        }

        [TestMethod]
        public async Task AddWithNickNameOutOfPattern()
        {
            var model = new AccountModel()
            {
                NickName = "Primeiro Usuario",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result, "O Nick Name deve conter apenas números, letras ou os símbolos _$#@!&.");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = new AccountModel()
            {
                NickName = "Mstest",
                Password = "12345678"
            };
            var result = await Post<AccountModel>("/api/account", model);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task GetDataOk()
        {
            var result = await Get<UserDataModel>("/api/account", 3);
            Assert.AreEqual("User3", result.Data.NickName);
        }
    }
}