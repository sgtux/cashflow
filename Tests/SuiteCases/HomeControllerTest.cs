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
        public async Task GetProjection()
        {
            var now = DateTime.Now;
            var payments = (await Get<List<PaymentMonthProjectionModel>>($"/api/Projection", 4)).Data;

            var paymentsMonth = payments.First(p => p.MonthYear == now.ToString("MM/yyyy"));

            // Earnings
            decimal expected = 2500;

            // Remaining Balance: 
            expected += 305; // (2500(Salary) - 2000(Installment) - 100 (FuelExpense) - 95 (RecurringExpense))

            // Installments 4/6
            expected -= 1500;

            // Household Expenses
            expected -= 300.5M;

            // FuelExpenses
            expected -= 200;

            // Recurring Expense
            expected -= 115; // 115/130

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