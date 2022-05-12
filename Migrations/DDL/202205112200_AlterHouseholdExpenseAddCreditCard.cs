using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202205112200)]
    public class AlterHouseholdExpenseAddCreditCard_202205112200 : Migration
    {
        public override void Up()
        {
            Alter.Table("HouseholdExpense")
                .AddColumn("CreditCardId").AsInt32().Nullable();

            Create.ForeignKey()
                .FromTable("HouseholdExpense").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Column("CreditCardId").FromTable("HouseholdExpense");
        }
    }
}