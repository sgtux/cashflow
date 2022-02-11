using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202202041200)]
    public class MigrationEarningFromSalary_202202041200 : Migration
    {
        public override void Up()
        {
            Execute.Sql($"INSERT INTO \"Earning\" (\"Description\", \"Value\", \"Date\", \"UserId\", \"Type\") SELECT 'Salário', \"Value\", CURRENT_DATE, \"UserId\", 1 FROM \"Salary\" WHERE \"EndDate\" IS NULL");
            Execute.Sql($"INSERT INTO \"Earning\" (\"Description\", \"Value\", \"Date\", \"UserId\", \"Type\") SELECT 'Salário', \"Value\", CURRENT_DATE - INTERVAL '1 MONTH', \"UserId\", 1 FROM \"Salary\" WHERE \"EndDate\" IS NULL");
            Execute.Sql($"INSERT INTO \"Earning\" (\"Description\", \"Value\", \"Date\", \"UserId\", \"Type\") SELECT 'Salário', \"Value\", CURRENT_DATE - INTERVAL '2 MONTH', \"UserId\", 1 FROM \"Salary\" WHERE \"EndDate\" IS NULL");
        }

        public override void Down()
        {
        }
    }
}