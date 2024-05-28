using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202405271200)]
    public class InsertSystemParameter_202405271200 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("SystemParameter").Row(new { Key = "MAXIMUM_SYSTEM_USERS", Value = "10", Type = "string" });
        }

        public override void Down()
        {
        }
    }
}