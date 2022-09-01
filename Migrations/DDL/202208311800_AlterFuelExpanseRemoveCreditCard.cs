using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202208311800)]
    public class AlterFuelExpanseRemoveCreditCard_202208311800 : Migration
    {
        public override void Up()
        {
            Delete.Column("CreditCardId").FromTable("FuelExpenses");
        }

        public override void Down()
        {
            Alter.Table("FuelExpenses")
                .AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey()
                .FromTable("FuelExpenses").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }
    }
}