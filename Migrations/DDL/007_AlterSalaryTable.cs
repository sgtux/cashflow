using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106172300)]
    public class AlterSalaryTable_202106172300 : Migration
    {
        public override void Up()
        {
            Alter.Table("Salary").AlterColumn("EndDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
        }
    }
}