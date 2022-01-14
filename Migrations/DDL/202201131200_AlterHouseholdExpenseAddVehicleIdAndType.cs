using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202201131200)]
    public class AlterHouseholdExpenseAddVehicleIdAndType_202201131200 : Migration
    {
        public override void Up()
        {
            Alter.Table("HouseholdExpense")
                .AddColumn("VehicleId").AsInt32().Nullable()
                .AddColumn("Type").AsInt32().WithDefaultValue(20);

            Create.ForeignKey()
                .FromTable("HouseholdExpense").ForeignColumn("VehicleId")
                .ToTable("Vehicle").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Column("VehicleId").FromTable("HouseholdExpense");
            Delete.Column("Type").FromTable("HouseholdExpense");
        }
    }
}