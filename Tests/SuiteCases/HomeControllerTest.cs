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

            // Salary
            decimal result = 2500;

            // Remaining Balance: 
            result += 305; // (2500(Salary) - 2000(Installment) - 100 (FuelExpense) - 95 (RecurringExpense))

            // Installments 4/6
            result -= 1500;

            // Household Expenses
            result -= 300.5M;

            // FuelExpenses
            result -= 200;

            // Recurring Expense
            result -= 115; // 115/130

            Assert.AreEqual(result, payments[now.ToString("MM/yyyy")].AccumulatedValue);
        }
    }
}