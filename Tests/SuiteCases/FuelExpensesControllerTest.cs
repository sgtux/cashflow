using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class FuelExpensesControllerTest : BaseControllerTest
    {
        private FuelExpenses ValidFuelExpense =>
        new FuelExpenses()
        {
            Date = new DateTime(2020, 12, 12),
            Miliage = 250,
            PricePerLiter = 7.5M,
            ValueSupplied = 50.5M,
            VehicleId = 3
        };

        [TestMethod]
        public async Task AddWithInvalidMinMiliage()
        {
            var model = ValidFuelExpense;
            model.Miliage = 0;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor mínimo para o campo 'Quilometragem' é 0.");
        }

        [TestMethod]
        public async Task AddWithInvalidMaxMiliage()
        {
            var model = ValidFuelExpense;
            model.Miliage = 1000000000;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor máximo para o campo 'Quilometragem' é 999999999.");
        }

        [TestMethod]
        public async Task AddWithInvalidMinPricePerLiter()
        {
            var model = ValidFuelExpense;
            model.PricePerLiter = 0;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor mínimo para o campo 'Preço por Litro' é 0.");
        }

        [TestMethod]
        public async Task AddWithInvalidMaxPricePerLiter()
        {
            var model = ValidFuelExpense;
            model.PricePerLiter = 1000000000;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor máximo para o campo 'Preço por Litro' é 999999999.");
        }

        [TestMethod]
        public async Task AddWithInvalidMinValueSupplied()
        {
            var model = ValidFuelExpense;
            model.ValueSupplied = 0;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor mínimo para o campo 'Valor Abastecido' é 0.");
        }

        [TestMethod]
        public async Task AddWithInvalidMaxValueSupplied()
        {
            var model = ValidFuelExpense;
            model.ValueSupplied = 1000000000;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O valor máximo para o campo 'Valor Abastecido' é 999999999.");
        }

        [TestMethod]
        public async Task AddWithInvalidDate()
        {
            var model = ValidFuelExpense;
            model.Date = new DateTime();
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithVehicleNotFound()
        {
            var model = ValidFuelExpense;
            model.VehicleId = 99;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "Veículo não encontrado(a).");
        }

        [TestMethod]
        public async Task AddWithMiliageIsNotMatch()
        {
            var model = ValidFuelExpense;
            model.Miliage = 200;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "Data e Quilometragem não batem devido à outro abastecimento");
        }

        [TestMethod]
        public async Task AddWithDataIsNotMatch()
        {
            var model = ValidFuelExpense;
            model.Date = new DateTime(2020, 12, 11);
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "Data e Quilometragem não batem devido à outro abastecimento");
        }

        [TestMethod]
        public async Task AddSameDayOk()
        {
            var model = ValidFuelExpense;
            model.Miliage = 180;
            model.Date = new DateTime(2020, 12, 11);
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = ValidFuelExpense;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateWithFuelExpenseNotFound()
        {
            var model = ValidFuelExpense;
            model.Id = 99;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result, "Despesa de combustível não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = ValidFuelExpense;
            model.Id = 6;
            model.Miliage = 210;
            var result = await Post("/api/FuelExpenses", model, 2);
            TestErrors(model, result);
        }
    }
}