using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class RecurringEarningControllerTest : BaseControllerTest
    {
        private RecurringEarningEntity DefaultRecurringEarning => new RecurringEarningEntity()
        {
            UserId = 2,
            Description = "Extra Income 2",
            Value = 327
        };

        private RecurringEarningHistoryEntity DefaultRecurringEarningHistory => new RecurringEarningHistoryEntity()
        {
            Id = 1,
            Value = 1750,
            RecurringEarningId = 1,
            Date = new DateTime(2020, 6, 1)
        };

        [TestMethod]
        public async Task GetByUserOk()
        {
            var result = await Get<IEnumerable<RecurringEarningEntity>>("/api/RecurringEarning?active=1", 4);
            Assert.IsTrue(result.Data.Any(p => p.Id == 4 && p.History.Any()));
        }

        [TestMethod]
        public async Task AddWithInvalidDescription()
        {
            var model = DefaultRecurringEarning;
            model.Description = "";
            var result = await Post("/api/RecurringEarning", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithInvalidValue()
        {
            var model = DefaultRecurringEarning;
            model.Value = 0;
            var result = await Post("/api/RecurringEarning", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = DefaultRecurringEarning;
            var result = await Post("/api/RecurringEarning", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateWithInvalidDescription()
        {
            var model = DefaultRecurringEarning;
            model.Id = 10;
            model.Description = "";
            var result = await Put($"/api/RecurringEarning/{model.Id}", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidValue()
        {
            var model = DefaultRecurringEarning;
            model.Id = 10;
            model.Value = 0;
            var result = await Put($"/api/RecurringEarning/{model.Id}", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidRecurringEarning()
        {
            var model = DefaultRecurringEarning;
            model.Id = 10;
            var result = await Put($"/api/RecurringEarning/{model.Id}", model, model.UserId);
            TestErrors(model, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateBelongsAnotherUser()
        {
            var model = DefaultRecurringEarning;
            model.Id = 1;
            model.UserId = 2;
            var result = await Put($"/api/RecurringEarning/{model.Id}", model, model.UserId);
            TestErrors(model, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = DefaultRecurringEarning;
            model.Id = 1;
            model.UserId = 1;
            var result = await Put($"/api/RecurringEarning/{model.Id}", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveNotFound()
        {
            var result = await Delete("/api/RecurringEarning/99", 1);
            TestErrors(new { Id = 99, UserId = 1 }, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveBelongsAnotherUser()
        {
            var result = await Delete("/api/RecurringEarning/1", 2);
            TestErrors(new { Id = 1, UserId = 2 }, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveOk()
        {
            var result = await Delete("/api/RecurringEarning/5", 1);
            TestErrors(new { Id = 5, UserId = 1 }, result);
        }

        [TestMethod]
        public async Task AddHistoryWithInvalidValue()
        {
            var model = DefaultRecurringEarningHistory;
            model.Value = 0;
            var result = await Post($"/api/RecurringEarning/History", model, 1);
            TestErrors(model, result, "O campo 'Valor' é obrigatório.");
        }

        [TestMethod]
        public async Task AddHistoryWithInvalidDate()
        {
            var model = DefaultRecurringEarningHistory;
            model.Date = new DateTime();
            var result = await Post($"/api/RecurringEarning/History", model, 1);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task AddHistoryWithRecurringEarningNotFound()
        {
            var model = DefaultRecurringEarningHistory;
            model.RecurringEarningId = 99;
            var result = await Post($"/api/RecurringEarning/History", model, 1);
            TestErrors(model, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task AddHistoryWithExistingMonthYear()
        {
            var model = DefaultRecurringEarningHistory;
            model.Id = 0;
            model.Date = new DateTime(2020, 11, 20);
            var result = await Post($"/api/RecurringEarning/History", model, 1);
            TestErrors(model, result, "Já existe um histórico para este Mês/Ano.");
        }

        [TestMethod]
        public async Task AddHistoryOk()
        {
            var model = DefaultRecurringEarningHistory;
            var result = await Post($"/api/RecurringEarning/History", model, 1);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateHistoryWithInvalidValue()
        {
            var model = DefaultRecurringEarningHistory;
            model.Value = 0;
            var result = await Put($"/api/RecurringEarning/History/{model.Id}", model, 1);
            TestErrors(model, result, "O campo 'Valor' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateHistoryWithInvalidDate()
        {
            var model = DefaultRecurringEarningHistory;
            model.Date = new DateTime();
            var result = await Put($"/api/RecurringEarning/History/{model.Id}", model, 1);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateHistoryWithRecurringEarningNotFound()
        {
            var model = DefaultRecurringEarningHistory;
            model.RecurringEarningId = 99;
            var result = await Put($"/api/RecurringEarning/History/{model.Id}", model, 1);
            TestErrors(model, result, "Provento Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateHistoryOk()
        {
            var model = DefaultRecurringEarningHistory;
            model.Date = new DateTime(2019, 1, 1);
            var result = await Put($"/api/RecurringEarning/History/{model.Id}", model, 1);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveHistoryOk()
        {
            var result = await Delete("/api/RecurringEarning/1/History/2", 1);
            TestErrors(new { Id = 2, UserId = 1 }, result);
        }
    }
}
