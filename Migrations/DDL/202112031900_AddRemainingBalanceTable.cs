using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112031900)]
    public class AddRemainingBalanceTable_202112031900 : Migration
    {
        public override void Up()
        {
            Create.Table("RemainingBalance")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2).NotNullable()
                .WithColumn("Month").AsInt16().NotNullable()
                .WithColumn("Year").AsInt16().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("RemainingBalance").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");

            Create.UniqueConstraint()
                .OnTable("RemainingBalance")
                .Columns("Month", "Year");
        }

        public override void Down()
        {
            Delete.Table("RemainigBalance");
        }
    }
}