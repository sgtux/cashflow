using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162009)]
    public class InsertRecurringExpenseHistory : Migration
    {
        public override void Up()
        {
            // Dia 1 do mês: no SQLite as datas do seed ficam como "yyyy-MM-ddTHH:mm:ss" e os filtros chegam como
            // "yyyy-MM-dd HH:mm:ss", então um registro no último dia do mês ficaria fora do "Date <= @EndDate".
            var month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Insert.IntoTable("RecurringExpenseHistory")
                .Row(new { Id = 1, PaidValue = 90.5, RecurringExpenseId = 1, Date = new DateTime(2020, 5, 5) })
                .Row(new { Id = 2, PaidValue = 90.5, RecurringExpenseId = 1, Date = new DateTime(2020, 6, 5) })
                .Row(new { Id = 3, PaidValue = 90.5, RecurringExpenseId = 2, Date = new DateTime(2020, 8, 10) })
                .Row(new { Id = 4, PaidValue = 30.5, RecurringExpenseId = 2, Date = new DateTime(2020, 9, 10) })
                .Row(new { Id = 5, PaidValue = 80.5, RecurringExpenseId = 2, Date = new DateTime(2020, 10, 10) })
                .Row(new { Id = 6, PaidValue = 80.5, RecurringExpenseId = 4, Date = new DateTime(2020, 10, 10) })
                .Row(new { Id = 7, PaidValue = 95, RecurringExpenseId = 5, Date = month.AddMonths(-1) })
                .Row(new { Id = 8, PaidValue = 115, RecurringExpenseId = 5, Date = month })
                .Row(new { Id = 9, PaidValue = 115, RecurringExpenseId = 6, Date = month });
        }

        public override void Down()
        {
        }
    }
}