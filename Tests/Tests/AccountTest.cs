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
        public async Task WithInvalidNickName()
        {
            var model = new AccountModel()
            {
                NickName = "",
                Password = "12345678"
            };
            var result = await _service.Add(model.Map(new User()));
            HasNotifications(result, "O campo 'Nick Name' deve ter pelo menos 4 caracteres.");
        }

        [TestMethod]
        public async Task WithInvalidPassword()
        {
            var model = new AccountModel()
            {
                NickName = "mstest",
                Password = "123"
            };
            var result = await _service.Add(model.Map(new User()));
            HasNotifications(result, "O campo 'Senha' deve ter pelo menos 8 caracteres.");
        }

        [TestMethod]
        public async Task WithNickNameAlreadyUsed()
        {
            var model = new AccountModel()
            {
                NickName = "Primeiro Usuário",
                Password = "12345678"
            };
            var result = await _service.Add(model.Map(new User()));
            HasNotifications(result, "O Nick Name informado já está sendo utilizado.");
        }

        [TestMethod]
        public async Task WithNickNameOutOfPattern()
        {
            var model = new AccountModel()
            {
                NickName = "Primeiro Usuário",
                Password = "12345678"
            };
            var result = await _service.Add(model.Map(new User()));
            HasNotifications(result, "O Nick Name deve conter apenas números, letras ou os símbolos _$#@!&.");
        }

        [TestMethod]
        public async Task CreateAccountOk()
        {
            var model = new AccountModel()
            {
                NickName = "Mstest",
                Password = "12345678"
            };
            var result = await _service.Add(model.Map(new User()));
            HasNotifications(result);
        }
    }
}