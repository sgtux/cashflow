using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202107041113)]
    public class AddVehicleTable : Migration
    {
        public override void Up()
        {
            Create.Table("Vehicle")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Vehicle");
        }
    }
}