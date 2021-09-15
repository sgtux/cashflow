using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141805)]
    public class AddPaymentTable : Migration
    {
        public override void Up()
        {
            Create.Table("Payment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("CreditCardId").AsInt32()
                .WithColumn("FixedPayment").AsBoolean()
                .WithColumn("Invoice").AsBoolean();

            Create.ForeignKey()
                .FromTable("Payment").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("Payment").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("Payment").ForeignColumn("Type")
                .ToTable("PaymentType").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Payment");
        }
    }
}