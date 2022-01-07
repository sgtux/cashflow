using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162009)]
    public class InsertRecurringExpenseHistory : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RecurringExpenseHistory")
                .Row(new { Id = 1, PaidValue = 90.5, RecurringExpenseId = 1, Date = new DateTime(2020, 5, 5) })
                .Row(new { Id = 2, PaidValue = 90.5, RecurringExpenseId = 1, Date = new DateTime(2020, 6, 5) })
                .Row(new { Id = 3, PaidValue = 90.5, RecurringExpenseId = 2, Date = new DateTime(2020, 8, 10) })
                .Row(new { Id = 4, PaidValue = 30.5, RecurringExpenseId = 2, Date = new DateTime(2020, 9, 10) })
                .Row(new { Id = 5, PaidValue = 80.5, RecurringExpenseId = 2, Date = new DateTime(2020, 10, 10) })
                .Row(new { Id = 6, PaidValue = 80.5, RecurringExpenseId = 4, Date = new DateTime(2020, 10, 10) });
        }

        public override void Down()
        {
        }
    }
}