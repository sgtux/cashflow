using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111171900)]
    public class AddFuelExpenseTable : Migration
    {
        public override void Up()
        {
            Create.Table("FuelExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Miliage").AsInt32().NotNullable()
                .WithColumn("ValueSupplied").AsDecimal(10, 2).NotNullable()
                .WithColumn("PricePerLiter").AsDecimal(10, 2).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("VehicleId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("FuelExpense");
        }
    }
}