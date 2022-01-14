using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162010)]
    public class InsertSalaries : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Salary")
                .Row(new { Id = 1, Value = 500, UserId = 1, StartDate = new DateTime(2019, 5, 1), EndDate = new DateTime(2019, 10, 1) })
                .Row(new { Id = 2, Value = 800, UserId = 1, StartDate = new DateTime(2019, 11, 1), EndDate = new DateTime(2019, 12, 1) })
                .Row(new { Id = 3, Value = 1200, UserId = 1, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 6, 1) })
                .Row(new { Id = 4, Value = 1500, UserId = 1, StartDate = new DateTime(2020, 7, 1), EndDate = new DateTime(2020, 8, 1) })
                .Row(new { Id = 5, Value = 2000, UserId = 1, StartDate = new DateTime(2020, 12, 1) })
                .Row(new { Id = 6, Value = 500, UserId = 2, StartDate = new DateTime(2019, 5, 1), EndDate = new DateTime(2019, 10, 1) })
                .Row(new { Id = 7, Value = 800, UserId = 2, StartDate = new DateTime(2019, 11, 1), EndDate = new DateTime(2019, 12, 1) })
                .Row(new { Id = 8, Value = 1200, UserId = 2, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 6, 1) })
                .Row(new { Id = 9, Value = 1500, UserId = 2, StartDate = new DateTime(2020, 7, 1), EndDate = new DateTime(2020, 8, 1) })
                .Row(new { Id = 10, Value = 2000, UserId = 2, StartDate = new DateTime(2020, 12, 1), EndDate = new DateTime(2021, 3, 1) })
                .Row(new { Id = 11, Value = 2000, UserId = 3, StartDate = new DateTime(2020, 12, 1), EndDate = new DateTime(2021, 3, 1) })
                .Row(new { Id = 12, Value = 2500, UserId = 4, StartDate = DateTime.Now.AddMonths(-2) });
        }

        public override void Down()
        {
        }
    }
}