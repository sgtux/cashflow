using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106230100)]
    public class AlterInstallmentTable_202106230100 : Migration
    {
        public override void Up()
        {
            Delete.Column("Paid").FromTable("Installment");
            Alter.Table("Installment").AddColumn("PaidDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PaidDate").FromTable("Installment");
            Alter.Table("Installment").AddColumn("Paid").AsBoolean();
        }
    }
}