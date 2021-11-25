using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111171900)]
    public class AddFuelExpensesTable : Migration
    {
        public override void Up()
        {
            Create.Table("FuelExpenses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Miliage").AsInt32().NotNullable()
                .WithColumn("ValueSupplied").AsDecimal(10, 2).NotNullable()
                .WithColumn("PricePerLiter").AsDecimal(10, 2).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("VehicleId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("FuelExpenses").ForeignColumn("VehicleId")
                .ToTable("Vehicle").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("FuelExpenses");
        }
    }
}