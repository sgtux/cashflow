using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112092330)]
    public class DropDailyExpensesItem_202112092330 : Migration
    {
        public override void Up()
        {
            Delete.Table("DailyExpensesItem");
        }

        public override void Down() { }
    }
}