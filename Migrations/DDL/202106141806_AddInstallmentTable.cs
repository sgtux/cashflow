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
                .WithColumn("Value").AsDecimal(10, 2).NotNullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Exempt").AsBoolean().WithDefaultValue(false)
                .WithColumn("PaidValue").AsDecimal(10, 2).Nullable()
                .WithColumn("PaidDate").AsDateTime().Nullable();

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