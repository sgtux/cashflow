using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class EarningControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task GetSalariesByUser()
        {
            var result = await Get<IEnumerable<Earning>>("/api/Earning", 1);
            Assert.IsTrue(result.Data.Count() > 0);
        }

        [TestMethod]
        public async Task GetTypes()
        {
            var result = await Get<IEnumerable<TypeModel>>("/api/Earning/Types", 1);
            Assert.IsTrue(result.Data.Count() == 2);
        }

        [TestMethod]
        public async Task AddEarningWithDefaultDate()
        {
            var model = new Earning()
            {
                Date = default(System.DateTime),
                Value = 1000,
                UserId = 1,
                Description = "Salário"
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        public async Task AddEarningWithoutDescription()
        {
            var model = new Earning()
            {
                Date = default(System.DateTime),
                Value = 1000,
                UserId = 1
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddSalaryWithValueNoMoreThenZero()
        {
            var model = new Earning()
            {
                Date = DateTime.Now,
                Value = 0,
                UserId = 1,
                Type = EarningType.Salary,
                Description = "Salário"
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddCurrentSalaryWithOtherCurrentSalary()
        {
            var model = new Earning()
            {
                Date = new DateTime(2020, 12, 1),
                Value = 1000,
                UserId = 1,
                Type = EarningType.Salary,
                Description = "Salário"
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result, "Já existe um salário para este mês/ano.");
        }

        [TestMethod]
        public async Task AddCurrentEarningWithInvalidType()
        {
            var model = new Earning()
            {
                Date = new DateTime(2021, 4, 1),
                Value = 1000,
                UserId = 2,
                Type = (EarningType)99,
                Description = "Salário"
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result, "Tipo inválido.");
        }

        [TestMethod]
        public async Task AddCurrentEarningOk()
        {
            var model = new Earning()
            {
                Date = new DateTime(2021, 4, 1),
                Value = 1000,
                UserId = 2,
                Type = EarningType.Salary,
                Description = "Salário"
            };
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task AddBenefitOk()
        {
            var model = new Earning()
            {
                Date = new DateTime(2020, 12, 1),
                Value = 1000,
                UserId = 1,
                Type = EarningType.Benefit,
                Description = "Salário"
            };
            Thread.Sleep(500);
            var result = await Post("/api/Earning", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateEarningWithDefaultDate()
        {
            var model = new Earning()
            {
                Date = default(System.DateTime),
                Value = 1000,
                UserId = 1,
                Id = 2,
                Description = "Salário"
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateEarningWithValueNoMoreThenZero()
        {
            var model = new Earning()
            {
                Id = 2,
                Date = DateTime.Now,
                Value = 0,
                UserId = 1,
                Type = EarningType.Benefit,
                Description = "Salário"
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task UpdateEarningWithoutDescription()
        {
            var model = new Earning()
            {
                Id = 2,
                Date = DateTime.Now,
                Value = 1000,
                UserId = 1,
                Type = EarningType.Benefit
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateCurrentSalaryWithOtherCurrentSalary()
        {
            var model = new Earning()
            {
                Id = 4,
                Description = "Salário",
                Date = new DateTime(2020, 7, 1),
                Value = 1000,
                UserId = 1,
                Type = EarningType.Salary
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "Já existe um salário para este mês/ano.");
        }

        [TestMethod]
        public async Task UpdateEarningNotFound()
        {
            var model = new Earning()
            {
                Id = 99,
                Description = "Salário",
                Date = new DateTime(2020, 4, 1),
                Value = 1000,
                UserId = 1
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "Benefício/Salário não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateEarningWithInvalidType()
        {
            var model = new Earning()
            {
                Id = 3,
                Description = "Salário",
                Date = new DateTime(2020, 4, 1),
                Value = 1000,
                UserId = 1,
                Type = (EarningType)99
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result, "Tipo inválido.");
        }

        [TestMethod]
        public async Task UpdateEarningOk()
        {
            var model = new Earning()
            {
                Id = 12,
                Description = "Salário",
                Date = new DateTime(2020, 4, 1),
                Value = 1000,
                UserId = 1,
                Type = EarningType.Salary
            };
            var result = await Put("/api/Earning", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveEarningNotFound()
        {
            var result = await Delete($"/api/Earning/99", 3);
            TestErrors(new { a = 99, b = 3 }, result, "Benefício/Salário não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveEarningOk()
        {
            var result = await Delete($"/api/Earning/32", 3);
            TestErrors(new { a = 11, b = 3 }, result);
        }
    }
}