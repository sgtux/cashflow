using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Tests.Mocks;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class VehicleControllerTest : BaseControllerTest
    {

        [TestMethod]
        public async Task AddWithInvalidDescription()
        {
            var model = new Vehicle()
            {
                Description = "",
                UserId = 1
            };
            var result = await Post("/api/Vehicle", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = new Vehicle()
            {
                Description = "Vehicle 2",
                UserId = 1
            };
            var result = await Post("/api/Vehicle", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateWithInvalidDescription()
        {
            var model = new Vehicle()
            {
                Description = "",
                UserId = 1,
                Id = 1
            };
            var result = await Put($"/api/Vehicle/{model.Id}", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = new Vehicle()
            {
                Description = "Vehicle 1",
                UserId = 1,
                Id = 1
            };
            var result = await Put($"/api/Vehicle/{model.Id}", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task MiliageTraveledOk()
        {
            var vehicle = await Get<Vehicle>("/api/Vehicle/2", 1);
            Assert.AreEqual(50, vehicle.Data.MiliageTraveled);
        }

        [TestMethod]
        public async Task MilagePerLiterOk()
        {
            var vehicle = await Get<Vehicle>("/api/Vehicle/2", 1);
            Assert.AreEqual(10.66M, vehicle.Data.MilagePerLiter);
        }
    }
}