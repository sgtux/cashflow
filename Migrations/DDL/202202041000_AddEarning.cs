using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202202041000)]
    public class AddEarning_202202041000 : Migration
    {
        public override void Up()
        {
            Create.Table("Earning")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime()
                .WithColumn("UserId").AsInt32()
                .WithColumn("Type").AsInt16();
        }

        public override void Down()
        {
            Delete.Table("Earning");
        }
    }
}