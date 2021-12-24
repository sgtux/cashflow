using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Tests.Mocks;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class SalaryControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task GetSalariesByUser()
        {
            var result = await Get<IEnumerable<Salary>>("/api/Salary", 1);
            Assert.IsTrue(result.Data.Count() > 0);
        }

        [TestMethod]
        public async Task AddSalaryWithDefaultStartDate()
        {
            var model = new Salary()
            {
                StartDate = default(System.DateTime),
                Value = 1000,
                UserId = 1
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result, "O campo 'Data Início' é obrigatório.");
        }

        [TestMethod]
        public async Task AddSalaryWithStartDateMoreThenEndDate()
        {
            var model = new Salary()
            {
                StartDate = DateTime.Now,
                EndDate = default(DateTime),
                Value = 1000,
                UserId = 1
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result, "A Data Fim deve ser maior que a Data Início.");
        }

        [TestMethod]
        public async Task AddSalaryWithValueNoMoreThenZero()
        {
            var model = new Salary()
            {
                StartDate = DateTime.Now,
                Value = 0,
                UserId = 1
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddCurrentSalaryWithOtherCurrentSalary()
        {
            var model = new Salary()
            {
                StartDate = new DateTime(2020, 12, 1),
                Value = 1000,
                UserId = 1
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result, "Tem outro salário vigente.");
        }

        [TestMethod]
        public async Task AddSalaryWithAnotherSalaryInThisDateRange()
        {
            var model = new Salary()
            {
                StartDate = new DateTime(2019, 5, 1),
                EndDate = new DateTime(2020, 6, 1).FixLastDayInMonth(),
                Value = 1000,
                UserId = 1
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result, "Tem outro salário neste intervalo de datas.");
        }

        [TestMethod]
        public async Task AddCurrentSalaryOk()
        {
            var model = new Salary()
            {
                StartDate = new DateTime(2021, 4, 1),
                Value = 1000,
                UserId = 2
            };
            var result = await Post("/api/Salary", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateSalaryWithDefaultStartDate()
        {
            var model = new Salary()
            {
                StartDate = default(System.DateTime),
                Value = 1000,
                UserId = 1,
                Id = 2
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "O campo 'Data Início' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithStartDateMoreThenEndDate()
        {
            var model = new Salary()
            {
                Id = 2,
                StartDate = DateTime.Now,
                EndDate = default(DateTime),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "A Data Fim deve ser maior que a Data Início.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithValueNoMoreThenZero()
        {
            var model = new Salary()
            {
                Id = 2,
                StartDate = DateTime.Now,
                Value = 0,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task UpdateCurrentSalaryWithOtherCurrentSalary()
        {
            var model = new Salary()
            {
                Id = 4,
                StartDate = new DateTime(2020, 7, 1),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "Tem outro salário vigente.");
        }

        [TestMethod]
        public async Task UpdateSalaryWithAnotherSalaryInThisDateRange()
        {
            var model = new Salary()
            {
                Id = 2,
                StartDate = new DateTime(2019, 7, 1),
                EndDate = new DateTime(2019, 12, 1).FixLastDayInMonth(),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "Tem outro salário neste intervalo de datas.");
        }

        [TestMethod]
        public async Task UpdateSalaryNotFound()
        {
            var model = new Salary()
            {
                Id = 99,
                StartDate = new DateTime(2020, 4, 1),
                EndDate = new DateTime(2020, 6, 1).FixLastDayInMonth(),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result, "Salário não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateSalaryOk()
        {
            var model = new Salary()
            {
                Id = 3,
                StartDate = new DateTime(2020, 4, 1),
                EndDate = new DateTime(2020, 6, 1).FixLastDayInMonth(),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Salary", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveSalaryNotFound()
        {
            var result = await Delete($"/api/Salary/99", 3);
            TestErrors(new { a = 99, b = 3 }, result, "Salário não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveSalaryOk()
        {
            var result = await Delete($"/api/Salary/11", 3);
            TestErrors(new { a = 11, b = 3 }, result);
        }
    }
}