using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Enums;
using Cashflow.Api.Models;
using Cashflow.Tests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public class HomeControllerTest : BaseControllerTest
    {
        [TestMethod]
        [TestCategory("Home")]
        public async Task GetProjection()
        {
            var now = DateTime.Now;
            var payments = (await Get<List<PaymentMonthProjectionModel>>($"/api/Projection", 4)).Data;

            var paymentsMonth = payments.First(p => p.MonthYear == now.ToString("MM/yyyy"));

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
            var expected = expectedPreviousValue + currentEarning - currentMonthFuelExpense - currentMonthHousehold - currentPayment - currentRecurring;

            decimal actual = payments.First(p => p.MonthYear == now.ToString("MM/yyyy")).AccumulatedValue;

            if (expected != actual)
            {
                var totalEarnings = paymentsMonth.Payments.Where(p => p.Type == MovementProjectionType.Earning).Sum(p => p.Value);
                var totalFuelExpenses = paymentsMonth.Payments.Where(p => p.Type == MovementProjectionType.FuelExpense).Sum(p => p.Value);
                var totalHouseholdExpenses = paymentsMonth.Payments.Where(p => p.Type == MovementProjectionType.HouseholdExpense).Sum(p => p.Value);
                var totalPayments = paymentsMonth.Payments.Where(p => p.Type == MovementProjectionType.Payment).Sum(p => p.Value);
                var totalRecurringExpenses = paymentsMonth.Payments.Where(p => p.Type == MovementProjectionType.RecurringExpenses).Sum(p => p.Value);

                var remainingBalance = paymentsMonth.Payments.First(p => p.Type == MovementProjectionType.RemainingBalanceIn || p.Type == MovementProjectionType.RemainingBalanceOut);
                var totalRemainingBalance = remainingBalance.In ? remainingBalance.Value : remainingBalance.Value * -1;

                System.Diagnostics.Trace.WriteLine($"Earnings: {totalEarnings}");
                System.Diagnostics.Trace.WriteLine($"Fuel Expense: {totalFuelExpenses}");
                System.Diagnostics.Trace.WriteLine($"Household Expense: {totalHouseholdExpenses}");
                System.Diagnostics.Trace.WriteLine($"Payments: {totalPayments}");
                System.Diagnostics.Trace.WriteLine($"Recurring Expense: {totalRecurringExpenses}");
                System.Diagnostics.Trace.WriteLine($"Remainig Balance: {totalRemainingBalance}");
            }

            Assert.AreEqual(expected, actual);
        }
    }
}