using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    [TestCategory("RemainingBalance")]
    public class RemainingBalanceControllerTest : BaseControllerTest
    {
        [TestMethod]
        public async Task GetPreviousMonth()
        {
            var previousMonth = DateTime.Now.AddMonths(-1);
            await Put("/api/RemainingBalance/Recalculate", new { previousMonth.Month, previousMonth.Year }, 4);
            var result = (await Get<List<RemainingBalance>>("/api/RemainingBalance", 4)).Data;
            var previousRemainingBalance = result.First(p => p.Date.SameMonthYear(previousMonth));
            var currentRemainingBalance = result.First(p => p.Date.SameMonthYear(p.CurrentDate));

            var previousMonthFuelExpense = 100;
            var currentMonthFuelExpense = 200;

            var previousMonthHousehold = 1000; // Day 3 - CreditCardId = 7, InvoiceDay 4
            var currentMonthHousehold = 300.5M + 1000 + 1000; // 1000 x 2 creditCard Days [4,5] - CreditCardId = 7, InvoiceDay 4

            var previousPayment = 2000;
            var currentPayment = 1500;

            var previousRecurring = 95;
            var currentRecurring = 115;

            var previousEarning = 2500;
            var currentEarning = 2500;

            var expectedPreviousValue = previousEarning - previousMonthFuelExpense - previousMonthHousehold - previousPayment - previousRecurring;
            var expectedCurrentValue = expectedPreviousValue + currentEarning - currentMonthFuelExpense - currentMonthHousehold - currentPayment - currentRecurring;

            Assert.AreEqual(expectedPreviousValue, previousRemainingBalance.Value); // -695
            Assert.AreEqual(expectedCurrentValue, currentRemainingBalance.Value); // -2310.5
        }
    }
}