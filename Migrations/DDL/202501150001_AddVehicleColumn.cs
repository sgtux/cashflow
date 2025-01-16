using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202501150001)]
    public class AddVehicleColumn_202501150001 : Migration
    {
        public override void Up()
        {
            Alter.Table("Vehicle").AddColumn("Active").AsBoolean().Nullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("Active").FromTable("Vehicle");
        }
    }
}