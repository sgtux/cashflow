using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class RecurringExpenseControllerTest : BaseControllerTest
    {
        private RecurringExpense DefaultRecurringExpense => new RecurringExpense()
        {
            UserId = 2,
            Description = "Computer Shop 2",
            Value = 327
        };

        private RecurringExpenseHistory DefaultRecurringExpenseHistory => new RecurringExpenseHistory()
        {
            Id = 6,
            PaidValue = 80.5M,
            RecurringExpenseId = 4,
            Date = new DateTime(2020, 11, 10)
        };

        [TestMethod]
        public async Task AddWithInvalidDescription()
        {
            var model = DefaultRecurringExpense;
            model.Description = "";
            var result = await Post("/api/RecurringExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task AddWithInvalidValue()
        {
            var model = DefaultRecurringExpense;
            model.Value = 0;
            var result = await Post("/api/RecurringExpense", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task AddWithInvalidCreditCard()
        {
            var model = DefaultRecurringExpense;
            model.CreditCardId = 99;
            var result = await Post("/api/RecurringExpense", model, model.UserId);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task AddOk()
        {
            var model = DefaultRecurringExpense;
            var result = await Post("/api/RecurringExpense", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateWithInvalidDescription()
        {
            var model = DefaultRecurringExpense;
            model.Id = 10;
            model.Description = "";
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result, "O campo 'Descrição' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidValue()
        {
            var model = DefaultRecurringExpense;
            model.Id = 10;
            model.Value = 0;
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result, "O campo 'Valor' deve ser maior que 0.");
        }

        [TestMethod]
        public async Task UpdateWithInvalidCreditCard()
        {
            var model = DefaultRecurringExpense;
            model.Id = 10;
            model.CreditCardId = 99;
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result, "Cartão de Crédito não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateWithInvalidRecurringExpense()
        {
            var model = DefaultRecurringExpense;
            model.Id = 10;
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateBelongsAnotherUser()
        {
            var model = DefaultRecurringExpense;
            model.Id = 1;
            model.UserId = 2;
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateOk()
        {
            var model = DefaultRecurringExpense;
            model.Id = 1;
            model.UserId = 1;
            var result = await Put($"/api/RecurringExpense/{model.Id}", model, model.UserId);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveNotFound()
        {
            var result = await Delete("/api/RecurringExpense/99", 1);
            TestErrors(new { Id = 99, UserId = 1 }, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveHasHistory()
        {
            var result = await Delete("/api/RecurringExpense/1", 1);
            TestErrors(new { Id = 1, UserId = 1 }, result, "A despesa recorrente possui histórico e não pode ser removida.");
        }

        [TestMethod]
        public async Task RemoveBelongsAnotherUser()
        {
            var result = await Delete("/api/RecurringExpense/3", 2);
            TestErrors(new { Id = 3, UserId = 2 }, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task RemoveOk()
        {
            var result = await Delete("/api/RecurringExpense/3", 1);
            TestErrors(new { Id = 3, UserId = 1 }, result);
        }

        [TestMethod]
        public async Task AddHistoryWithInvalidValue()
        {
            var model = DefaultRecurringExpenseHistory;
            model.PaidValue = 0;
            var result = await Post($"/api/RecurringExpense/History", model, 1);
            TestErrors(model, result, "O campo 'Valor Pago' é obrigatório.");
        }

        [TestMethod]
        public async Task AddHistoryWithInvalidDate()
        {
            var model = DefaultRecurringExpenseHistory;
            model.Date = new DateTime();
            var result = await Post($"/api/RecurringExpense/History", model, 1);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task AddHistoryWithRecurringExpenseNotFound()
        {
            var model = DefaultRecurringExpenseHistory;
            model.RecurringExpenseId = 99;
            var result = await Post($"/api/RecurringExpense/History", model, 1);
            TestErrors(model, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task AddHistoryOk()
        {
            var model = DefaultRecurringExpenseHistory;
            var result = await Post($"/api/RecurringExpense/History", model, 1);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task UpdateHistoryWithInvalidValue()
        {
            var model = DefaultRecurringExpenseHistory;
            model.PaidValue = 0;
            var result = await Put($"/api/RecurringExpense/History/{model.Id}", model, 1);
            TestErrors(model, result, "O campo 'Valor Pago' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateHistoryWithInvalidDate()
        {
            var model = DefaultRecurringExpenseHistory;
            model.Date = new DateTime();
            var result = await Put($"/api/RecurringExpense/History/{model.Id}", model, 1);
            TestErrors(model, result, "O campo 'Data' é obrigatório.");
        }

        [TestMethod]
        public async Task UpdateHistoryWithRecurringExpenseNotFound()
        {
            var model = DefaultRecurringExpenseHistory;
            model.RecurringExpenseId = 99;
            var result = await Put($"/api/RecurringExpense/History/{model.Id}", model, 1);
            TestErrors(model, result, "Despesa Recorrente não encontrado(a).");
        }

        public async Task UpdateHistoryWithHistoryNotFound()
        {
            var model = DefaultRecurringExpenseHistory;
            model.Id = 99;
            var result = await Put($"/api/RecurringExpense/History/{model.Id}", model, 1);
            TestErrors(model, result, "Despesa Recorrente não encontrado(a).");
        }

        [TestMethod]
        public async Task UpdateHistoryOk()
        {
            var model = DefaultRecurringExpenseHistory;
            model.Date = new DateTime(2022, 1, 1);
            var result = await Put($"/api/RecurringExpense/History/{model.Id}", model, 1);
            TestErrors(model, result);
        }

        [TestMethod]
        public async Task RemoveHistoryOk()
        {
            var result = await Delete("/api/RecurringExpense/4/History/6", 1);
            TestErrors(new { Id = 6, UserId = 1 }, result);
        }
    }
}