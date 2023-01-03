using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202301031200)]
    public class AlterHouseholdExpenseRemoveCreditCard_202301031200 : Migration
    {
        public override void Up()
        {
            Delete.Column("CreditCardId").FromTable("HouseholdExpense");
        }

        public override void Down()
        {
            Alter.Table("HouseholdExpense")
                .AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey()
                .FromTable("HouseholdExpense").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }
    }
}