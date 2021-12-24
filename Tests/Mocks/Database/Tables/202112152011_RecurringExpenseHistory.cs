using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152011)]
    public class RecurringExpenseHistory : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringExpenseHistory")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaidValue").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime();

            Execute.Sql("ALTER TABLE RecurringExpenseHistory ADD COLUMN RecurringExpenseId INTEGER REFERENCES RecurringExpense(Id)");
        }

        public override void Down()
        {
        }
    }
}