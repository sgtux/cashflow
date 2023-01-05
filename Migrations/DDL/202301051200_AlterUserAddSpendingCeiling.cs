using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202301051200)]
    public class AlterUserAddSpendingCeiling_202301051200 : Migration
    {
        public override void Up()
        {
            Alter.Table("User")
                .AddColumn("SpendingCeiling").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SpendingCeiling").FromTable("User");
        }
    }
}