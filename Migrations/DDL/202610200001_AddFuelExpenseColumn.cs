using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202610200001)]
    public class AddFuelExpenseColumn_202610200001 : Migration
    {
        public override void Up()
        {
            Alter.Table("FuelExpense").AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey("FK_FuelExpense_CreditCard")
                .FromTable("FuelExpense").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Column("CreditCardId").FromTable("FuelExpense");
        }
    }
}