using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162008)]
    public class InsertRecurringExpenses : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RecurringExpense")
                .Row(new { Id = 1, Description = "Recurring Expense 1", UserId = 1, Value = 82 })
                .Row(new { Id = 2, Description = "Recurring Expense 2", UserId = 1, Value = 82 })
                .Row(new { Id = 3, Description = "Recurring Expense 3", UserId = 1, Value = 82 });
        }

        public override void Down()
        {
        }
    }
}