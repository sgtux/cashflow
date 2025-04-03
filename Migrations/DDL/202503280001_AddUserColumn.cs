using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202503280001)]
    public class AddUserColumn_202503280001 : Migration
    {
        public override void Up()
        {
            Alter.Table("User").AddColumn("Plan").AsByte().NotNullable().WithDefaultValue(1);
            Alter.Table("User").AddColumn("RecordsUsed").AsInt32().NotNullable().WithDefaultValue(1);
        }

        public override void Down()
        {
            Delete.Column("Plan").FromTable("User");
            Delete.Column("RecordsUsed").FromTable("User");
        }
    }
}