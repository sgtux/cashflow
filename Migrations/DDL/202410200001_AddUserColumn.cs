using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202410200001)]
    public class AddUserColumn_202410200001 : Migration
    {
        public override void Up()
        {
            Alter.Table("User").AddColumn("FuelExpenseLimit").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Column("FuelExpenseLimit").FromTable("User");
        }
    }
}