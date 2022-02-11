using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202202041400)]
    public class DropSalary_202202041400 : Migration
    {
        public override void Up()
        {
            Delete.Table("Salary");
        }

        public override void Down() { }
    }
}