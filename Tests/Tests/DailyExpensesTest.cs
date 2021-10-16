using System;
using System.Collections.Generic;
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
    public class DailyExpensesTest : BaseTest
    {
        private DailyExpensesService _service;

        private DailyExpenses ValidDailyExpenses = new DailyExpenses()
        {
            Id = 10,
            UserId = 2,
            ShopName = "Computer Shop 2",
            Date = new DateTime(2019, 5, 1),
            Items = new List<DailyExpensesItem>()
            {
                new DailyExpensesItem() { DailyExpensesId = 1, ItemName = "Mouse 2", Price = 67 },
                new DailyExpensesItem() { DailyExpensesId = 2, ItemName = "Processor 2", Price = 327.86M }
            }
        };

        [TestInitialize]
        public void Init()
        {
            _service = new DailyExpensesService(new DailyExpensesRepositoryMock());
        }

        [TestMethod]
        public async Task WithInvalidShopName()
        {
            var expense = ValidDailyExpenses;
            expense.ShopName = string.Empty;
            var result = await _service.Add(expense);
            HasNotifications(result, "O campo 'Nick Name' deve ter pelo menos 4 caracteres.");
        }
    }
}