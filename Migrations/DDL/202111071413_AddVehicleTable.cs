using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111071413)]
    public class AddVehicleTable : Migration
    {
        public override void Up()
        {
            Create.Table("Vehicle")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("Vehicle").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Vehicle");
        }
    }
}