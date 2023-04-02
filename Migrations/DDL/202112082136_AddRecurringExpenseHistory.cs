using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112082136)]
    public class AddRecurringExpenseHistory_202112082136 : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringExpenseHistory")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaidValue").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime()
                .WithColumn("RecurringExpenseId").AsInt32();

            Create.ForeignKey("FK_RecurringExpenseHistory_RecurringExpense")
                .FromTable("RecurringExpenseHistory").ForeignColumn("RecurringExpenseId")
                .ToTable("RecurringExpense").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("RecurringExpenseHistory");
        }
    }
}