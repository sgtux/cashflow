using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Models;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class HomeControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task GetProjection()
        {
            var date = DateTime.Now.AddMonths(10);
            var payments = (await Get<Dictionary<string, PaymentProjectionResultModel>>($"/api/Home/Projection?month={date.Month}&year={date.Year}", 4)).Data;

            var now = DateTime.Now;

            Assert.AreEqual(11, payments.Count);

            // Salary: 2500
            // Remaining Balance: 305 (2500(Salary) - 2000(Installment) - 100 (FuelExpense) - 95 (RecurringExpense))
            // Installments 4/6: 2000
            // Household Expenses: 300.5
            // FuelExpenses: 200
            // Recurring Expense: 115/130
            Assert.AreEqual((decimal)(2500 + 305 - 1500 - 300.5 - 200 - 115), payments[now.ToString("MM/yyyy")].AccumulatedValue);
        }
    }
}