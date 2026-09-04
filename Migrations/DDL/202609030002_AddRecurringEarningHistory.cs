using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202609030002)]
    public class AddRecurringEarningHistory_202609030002 : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringEarningHistory")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime()
                .WithColumn("RecurringEarningId").AsInt32();
        }

        public override void Down()
        {
            Delete.Table("RecurringEarningHistory");
        }
    }
}
