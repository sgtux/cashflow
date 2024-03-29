using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152008)]
    public class FuelExpense : Migration
    {
        public override void Up()
        {
            Create.Table("FuelExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Miliage").AsInt32().NotNullable()
                .WithColumn("ValueSupplied").AsDecimal(10, 2).NotNullable()
                .WithColumn("PricePerLiter").AsDecimal(10, 2).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();

            Execute.Sql("ALTER TABLE FuelExpense ADD COLUMN VehicleId INTEGER REFERENCES Vehicle(Id)");
        }

        public override void Down()
        {
        }
    }
}