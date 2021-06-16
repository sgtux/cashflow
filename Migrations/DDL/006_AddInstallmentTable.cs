using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141806)]
    public class AddInstallmentTable : Migration
    {
        public override void Up()
        {
            Create.Table("Installment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaymentId").AsInt32().NotNullable()
                .WithColumn("Cost").AsDecimal().NotNullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Paid").AsBoolean();

            Create.ForeignKey()
                .FromTable("Installment").ForeignColumn("PaymentId")
                .ToTable("Payment").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Installment");
        }
    }
}