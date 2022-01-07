using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112092300)]
    public class UpdateHouseholdExpense_202112092300 : Migration
    {
        public override void Up()
        {
            Execute.Sql($"UPDATE \"HouseholdExpense\" SET \"Description\" = \"Description\" || ' - ' || subquery.description, \"Value\" = subquery.value FROM (SELECT \"DailyExpensesId\", SUM(\"Price\" * \"Amount\") AS Value, string_agg(\"ItemName\", ',') AS Description FROM \"DailyExpensesItem\" GROUP BY \"DailyExpensesId\") AS subquery WHERE \"Id\" = subquery.\"DailyExpensesId\"");
        }

        public override void Down()
        {
        }
    }
}