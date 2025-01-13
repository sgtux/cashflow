using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162004)]
    public class InsertHouseholdExpenses : Migration
    {
        public override void Up()
        {
            var now = DateTime.Now;
            Insert.IntoTable("HouseholdExpense")
                .Row(new { Description = "Computer Shop", UserId = 1, Value = 256.5, Date = new DateTime(2019, 05, 01), Type = 10 })
                .Row(new { Description = "Computer Shop 2", UserId = 1, Value = 256.5, Date = new DateTime(2019, 05, 01), Type = 10, CreditCardId = 6 })
                .Row(new { Description = "Computer Shop 3", UserId = 4, Value = 300.5, Date = new DateTime(now.Year, now.Month, 1), Type = 10 })
                .Row(new { Description = "Computer Shop 4", UserId = 4, Value = 1000, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 3), Type = 10, CreditCardId = 7 })
                .Row(new { Description = "Computer Shop 5", UserId = 4, Value = 1000, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 4), Type = 10, CreditCardId = 7 })
                .Row(new { Description = "Computer Shop 6", UserId = 4, Value = 1000, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 5), Type = 10, CreditCardId = 7 });
        }

        public override void Down()
        {
        }
    }
}