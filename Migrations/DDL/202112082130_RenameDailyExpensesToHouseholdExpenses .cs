using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112082130)]
    public class RenameDailyExpensesToHouseholdExpenses_202112082130 : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE \"DailyExpenses\" RENAME TO \"HouseholdExpense\"");
        }

        public override void Down()
        {
            Delete.Table("ALTER TABLE \"HouseholdExpense\" RENAME TO \"DailyExpenses\"");
        }
    }
}