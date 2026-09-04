using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202609030001)]
    public class AddRecurringEarning_202609030001 : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringEarning")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("InactiveAt").AsDateTime().Nullable()
                .WithColumn("UserId").AsInt32();
        }

        public override void Down()
        {
            Delete.Table("RecurringEarning");
        }
    }
}
