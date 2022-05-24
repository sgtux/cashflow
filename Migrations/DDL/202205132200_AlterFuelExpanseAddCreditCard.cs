using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202205132200)]
    public class AlterFuelExpanseAddCreditCard_202205132200 : Migration
    {
        public override void Up()
        {
            Alter.Table("FuelExpenses")
                .AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey()
                .FromTable("FuelExpenses").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Column("CreditCardId").FromTable("FuelExpenses");
        }
    }
}