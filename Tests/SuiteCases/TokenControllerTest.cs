using System.Threading.Tasks;
using Cashflow.Api.Models.Account;
using Cashflow.Tests.TestModels;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestCategory("Token")]
    [TestClass]
    public class TokenControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task LoginInvalid()
        {
            var user = new AccountModel()
            {
                Email = "User1@mail.com",
                Password = "123488"
            };

            var result = await Post<ApiResultDataModel<AccountResultModel>>("/api/token", user);

            TestErrors(user, result, "Usuário ou senha inválidos.");
        }

        [TestMethod]
        public async Task LoginOk()
        {
            var user = new AccountModel()
            {
                Email = "User1@mail.com",
                Password = "12345678"
            };

            var result = await Post<AccountResultModel>("/api/token", user);

            Assert.IsFalse(string.IsNullOrEmpty(result.Data?.Token));
            TestErrors(user, result);
        }
    }
}