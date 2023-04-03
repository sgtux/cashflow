using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202109151300)]
    public class AddHouseholdExpenseTable : Migration
    {
        public override void Up()
        {
            Create.Table("HouseholdExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("VehicleId").AsInt32().Nullable()
                .WithColumn("Type").AsInt32().WithDefaultValue(20)
                .WithColumn("Value").AsDecimal(10, 2).WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Table("HouseholdExpense");
        }
    }
}