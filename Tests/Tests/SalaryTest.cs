using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Cashflow.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class SalaryTest : BaseTest
    {
        private SalaryService _service;

        [TestInitialize]
        public void Init() => _service = new SalaryService(new SalaryRepositoryMock());

        [TestMethod]
        public async Task GetSalariesByUser()
        {
            var result = await _service.GetByUser(1);
            Assert.IsTrue(result.Data.Count() > 0);
        }

        [TestMethod]
        public async Task InsertSalaryWithDefaultStartDate()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = default(System.DateTime),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "O campo Data Início é inválido.");
        }

        [TestMethod]
        public async Task InsertSalaryWithStartDateMoreThenEndDate()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = DateTime.Now,
                EndDate = default(DateTime),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "A Data Fim deve ser maior que a Data Início.");
        }

        [TestMethod]
        public async Task InsertSalaryWithValueNoMoreThenZero()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = DateTime.Now,
                Value = 0,
                UserId = 1
            });
            HasNotifications(result, "O campo Valor deve ser maior que zero.");
        }

        [TestMethod]
        public async Task InsertCurrentSalaryWithOtherCurrentSalary()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = new DateTime(2020, 12, 1),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "Tem outro salário vigente.");
        }

        [TestMethod]
        public async Task InsertSalaryWithAnotherSalaryInThisDateRange()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = new DateTime(2019, 5, 1),
                EndDate = Utils.CreateEndDate(2020, 6),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "Tem outro salário neste intervalo de datas.");
        }

        [TestMethod]
        public async Task InsertCurrentSalaryOk()
        {
            var result = await _service.Add(new Salary()
            {
                StartDate = new DateTime(2021, 4, 1),
                Value = 1000,
                UserId = 2
            });
            HasNotifications(result);
        }

        [TestMethod]
        public async Task UpdateSalaryWithDefaultStartDate()
        {
            var result = await _service.Update(new Salary()
            {
                StartDate = default(System.DateTime),
                Value = 1000,
                UserId = 1,
                Id = 2
            });
            HasNotifications(result, "O campo Data Início é inválido.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithStartDateMoreThenEndDate()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 2,
                StartDate = DateTime.Now,
                EndDate = default(DateTime),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "A Data Fim deve ser maior que a Data Início.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithValueNoMoreThenZero()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 2,
                StartDate = DateTime.Now,
                Value = 0,
                UserId = 1
            });
            HasNotifications(result, "O campo Valor deve ser maior que zero.");
        }

        [TestMethod]
        public async Task UpdateCurrentSalaryWithOtherCurrentSalary()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 4,
                StartDate = new DateTime(2020, 7, 1),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "Tem outro salário vigente.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithAnotherSalaryInThisDateRange()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 2,
                StartDate = new DateTime(2019, 7, 1),
                EndDate = Utils.CreateEndDate(2019, 12),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "Tem outro salário neste intervalo de datas.");
        }

        [TestMethod]
        public async Task UpdateSalaryNotFound()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 99,
                StartDate = new DateTime(2020, 4, 1),
                EndDate = Utils.CreateEndDate(2020, 6),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result, "Salário não encontrado.");
        }

        [TestMethod]
        public async Task UpdateSalaryOk()
        {
            var result = await _service.Update(new Salary()
            {
                Id = 3,
                StartDate = new DateTime(2020, 4, 1),
                EndDate = Utils.CreateEndDate(2020, 6),
                Value = 1000,
                UserId = 1
            });
            HasNotifications(result);
        }

        [TestMethod]
        public async Task RemoveSalaryNotFound()
        {
            var result = await _service.Remove(99, 3);
            HasNotifications(result, "Salário não encontrado.");
        }

        [TestMethod]
        public async Task RemoveSalaryOk()
        {
            var result = await _service.Remove(11, 3);
            HasNotifications(result);
        }
    }
}