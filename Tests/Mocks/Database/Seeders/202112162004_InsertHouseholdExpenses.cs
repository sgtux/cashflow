using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162004)]
    public class InsertHouseholdExpenses : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("HouseholdExpense")
                .Row(new { Description = "Computer Shop", UserId = 1, Value = 256.5, Date = new DateTime(2019, 05, 01) })
                .Row(new { Description = "Computer Shop 2", UserId = 1, Value = 256.5, Date = new DateTime(2019, 05, 01) });
        }

        public override void Down()
        {
        }
    }
}