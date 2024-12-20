using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202412150001)]
    public class AddHouseholdExpenseColumn_202412150001 : Migration
    {
        public override void Up()
        {
            Alter.Table("HouseholdExpense").AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey("FK_HouseholdExpense_CreditCard")
                .FromTable("HouseholdExpense").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Column("CreditCardId").FromTable("HouseholdExpense");
        }
    }
}