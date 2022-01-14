using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202201141900)]
    public class AlterRemainingBalanceTable_202201141900 : Migration
    {
        public override void Up()
        {
            Delete.UniqueConstraint("UC_RemainingBalance_Month_Year").FromTable("RemainingBalance");

            Create.UniqueConstraint()
                .OnTable("RemainingBalance")
                .Columns("UserId", "Month", "Year");
        }

        public override void Down() { }
    }
}