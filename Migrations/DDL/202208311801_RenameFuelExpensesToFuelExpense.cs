using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202208311801)]
    public class RenameFuelExpensesToFuelExpense_202208311801 : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE \"FuelExpenses\" RENAME TO \"FuelExpense\"");
        }

        public override void Down()
        {
            Delete.Table("ALTER TABLE \"FuelExpense\" RENAME TO \"FuelExpenses\"");
        }
    }
}