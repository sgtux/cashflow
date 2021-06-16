using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141804)]
    public class AddPaymentTypeTable : Migration
    {
        public override void Up()
        {
            Create.Table("PaymentType")
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Description").AsString(100).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("PaymentType");
        }
    }
}