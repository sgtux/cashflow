using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models.Account;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    [TestCategory("Account")]
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

        [TestMethod]
        public async Task UpdateWithMinExpenseLimitError()
        {
            var model = new User()
            {
                ExpenseLimit = -1,
                FuelExpenseLimit = 99
            };
            var result = await Put("/api/account", model, 3);
            TestErrors(model, result, "O valor do campo 'ExpenseLimit' deve estar entre 0 e 99999.");
        }

        [TestMethod]
        public async Task UpdateWithMaxExpenseLimitError()
        {
            var model = new User()
            {
                ExpenseLimit = 999999,
                FuelExpenseLimit = 99
            };
            var result = await Put("/api/account", model, 3);
            TestErrors(model, result, "O valor do campo 'ExpenseLimit' deve estar entre 0 e 99999.");
        }

        [TestMethod]
        public async Task UpdateWithMinFuelExpenseLimitError()
        {
            var model = new User()
            {
                ExpenseLimit = 99,
                FuelExpenseLimit = -1
            };
            var result = await Put("/api/account", model, 3);
            TestErrors(model, result, "O valor do campo 'FuelExpenseLimit' deve estar entre 0 e 99999.");
        }

        [TestMethod]
        public async Task UpdateWithMaxFuelExpenseLimitError()
        {
            var model = new User()
            {
                ExpenseLimit = 99,
                FuelExpenseLimit = 999999
            };
            var result = await Put("/api/account", model, 3);
            TestErrors(model, result, "O valor do campo 'FuelExpenseLimit' deve estar entre 0 e 99999.");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = new User()
            {
                ExpenseLimit = 333.33m,
                FuelExpenseLimit = 444.44m
            };
            var result = await Put("/api/account", model, 3);

            TestErrors(model, result);

            var userResult = await Get<UserDataModel>("/api/account", 3);
            Assert.AreEqual(model.ExpenseLimit, userResult.Data.ExpenseLimit);
            Assert.AreEqual(model.FuelExpenseLimit, userResult.Data.FuelExpenseLimit);
        }
    }
}