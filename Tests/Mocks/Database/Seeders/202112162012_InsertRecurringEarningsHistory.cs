using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162012)]
    public class InsertRecurringEarningsHistory : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RecurringEarningHistory")
                .Row(new { Id = 1, Value = 1500, RecurringEarningId = 1, Date = new DateTime(2020, 11, 1) })
                .Row(new { Id = 2, Value = 2000, RecurringEarningId = 1, Date = DateTime.Now.AddMonths(-1) })
                .Row(new { Id = 3, Value = 2000, RecurringEarningId = 1, Date = DateTime.Now })
                .Row(new { Id = 4, Value = 2000, RecurringEarningId = 2, Date = DateTime.Now.AddMonths(-1) })
                .Row(new { Id = 5, Value = 2000, RecurringEarningId = 2, Date = DateTime.Now })
                .Row(new { Id = 6, Value = 2000, RecurringEarningId = 3, Date = new DateTime(2020, 12, 1) })
                .Row(new { Id = 7, Value = 2500, RecurringEarningId = 4, Date = DateTime.Now.AddMonths(-2) })
                .Row(new { Id = 8, Value = 2500, RecurringEarningId = 4, Date = DateTime.Now.AddMonths(-1) })
                .Row(new { Id = 9, Value = 2500, RecurringEarningId = 4, Date = DateTime.Now })
                .Row(new { Id = 10, Value = 1000, RecurringEarningId = 5, Date = DateTime.Now });
        }

        public override void Down()
        {
        }
    }
}
