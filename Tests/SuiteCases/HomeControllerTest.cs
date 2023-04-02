using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var now = DateTime.Now;
            var date = now.AddMonths(10);
            var payments = (await Get<List<PaymentMonthProjectionModel>>($"/api/Projection?month={date.Month}&year={date.Year}", 4)).Data;


            Assert.AreEqual(11, payments.Count);

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
                var totalEarnings = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.Earning).Sum(p => p.Value);
                var totalFuelExpenses = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.FuelExpense).Sum(p => p.Value);
                var totalHouseholdExpenses = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.HouseholdExpense).Sum(p => p.Value);
                var totalPayments = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.Payment).Sum(p => p.Value);
                var totalRecurringExpenses = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.RecurringExpenses).Sum(p => p.Value);
                var totalRemainingBalance = paymentsMonth.Payments.Where(p => p.Type == Api.Enums.MovementProjectionType.RemainingBalance).Sum(p => p.Value);

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