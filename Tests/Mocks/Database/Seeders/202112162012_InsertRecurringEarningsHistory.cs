using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162012)]
    public class InsertRecurringEarningsHistory : Migration
    {
        public override void Up()
        {
            // Dia 1 do mês: no SQLite as datas do seed ficam como "yyyy-MM-ddTHH:mm:ss" e os filtros chegam como
            // "yyyy-MM-dd HH:mm:ss", então um registro no último dia do mês ficaria fora do "Date <= @EndDate".
            var month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Insert.IntoTable("RecurringEarningHistory")
                .Row(new { Id = 1, Value = 1500, RecurringEarningId = 1, Date = new DateTime(2020, 11, 1) })
                .Row(new { Id = 2, Value = 2000, RecurringEarningId = 1, Date = month.AddMonths(-1) })
                .Row(new { Id = 3, Value = 2000, RecurringEarningId = 1, Date = month })
                .Row(new { Id = 4, Value = 2000, RecurringEarningId = 2, Date = month.AddMonths(-1) })
                .Row(new { Id = 5, Value = 2000, RecurringEarningId = 2, Date = month })
                .Row(new { Id = 6, Value = 2000, RecurringEarningId = 3, Date = new DateTime(2020, 12, 1) })
                .Row(new { Id = 7, Value = 2500, RecurringEarningId = 4, Date = month.AddMonths(-2) })
                .Row(new { Id = 8, Value = 2500, RecurringEarningId = 4, Date = month.AddMonths(-1) })
                .Row(new { Id = 9, Value = 2500, RecurringEarningId = 4, Date = month })
                .Row(new { Id = 10, Value = 1000, RecurringEarningId = 5, Date = month });
        }

        public override void Down()
        {
        }
    }
}
