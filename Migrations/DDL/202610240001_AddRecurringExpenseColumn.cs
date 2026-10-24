using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202610240001)]
    public class AddRecurringExpenseColumn_202610240001 : Migration
    {
        public override void Up()
        {
            Alter.Table("RecurringExpense").AddColumn("Type").AsInt32().NotNullable().WithDefaultValue(20);
        }

        public override void Down()
        {
            Delete.Column("Type").FromTable("RecurringExpense");
        }
    }
}
