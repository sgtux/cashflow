using System;
using System.Threading.Tasks;
using Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class HouseholdExpenseControllerTest : BaseControllerTest
    {
        private HouseholdExpense DefaultHouseholdExpense = new HouseholdExpense()
        {
            Id = 10,
            UserId = 2,
            Description = "Computer Shop 3",
            Date = new DateTime(2019, 5, 1),
            Value = 327.5M,
            VehicleId = 3,
            Type = ExpenseType.Others
        };

        [TestMethod]
        public async Task AddWithInvalidDescription()
        {
            var model = DefaultHouseholdExpense;
            model.Description = string.Empty;
            var result = await Post("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithInvalidDate()
        {
            var model = DefaultHouseholdExpense;
            model.Date = new DateTime();
            var result = await Post("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithInvalidValue()
        {
            var model = DefaultHouseholdExpense;
            model.Value = 0;
            var result = await Post("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddWithInvalidVehicle()
        {
            var model = DefaultHouseholdExpense;
            model.VehicleId = 2;
            var result = await Post("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "Veículo não encontrado(a).");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = DefaultHouseholdExpense;
            var result = await Post("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateWithInvalidDescription()
        {
            var model = DefaultHouseholdExpense;
            model.Description = string.Empty;
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidDate()
        {
            var model = DefaultHouseholdExpense;
            model.Date = new DateTime();
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidValue()
        {
            var model = DefaultHouseholdExpense;
            model.Value = 0;
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidVehicle()
        {
            var model = DefaultHouseholdExpense;
            model.VehicleId = 2;
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "Veículo não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateWithInvalidCreditCard()
        {
            var model = DefaultHouseholdExpense;
            model.CreditCardId = 99;
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = DefaultHouseholdExpense;
            var result = await Put("/api/HouseholdExpense", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveWithNotFound()
        {
            var result = await Delete($"/api/HouseholdExpense/1", 2);
            TestErrors(new { Id = 1, UserId = 2 }, result, "Despesa Doméstica não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveOk()
        {
            var result = await Delete($"/api/HouseholdExpense/2", 1);
            TestErrors(new { Id = 2, UserId = 1 }, result);
        }
    }
}