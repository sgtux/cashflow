using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141801)]
    public class AddSalaryTable : Migration
    {
        public override void Up()
        {
            Create.Table("Salary")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("Value").AsDecimal(10, 2).NotNullable()
                .WithColumn("StartDate").AsDateTime().NotNullable()
                .WithColumn("EndDate").AsDateTime();

            Create.ForeignKey()
                .FromTable("Salary").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Salary");
        }
    }
}