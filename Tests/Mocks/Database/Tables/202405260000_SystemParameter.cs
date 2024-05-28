using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202405260000)]
    public class AddSystemParameter_202405260000 : Migration
    {
        public override void Up()
        {
            Create.Table("SystemParameter")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Key").AsString(32)
                .WithColumn("Value").AsString(32)
                .WithColumn("Type").AsString(8);
        }

        public override void Down() { }
    }
}