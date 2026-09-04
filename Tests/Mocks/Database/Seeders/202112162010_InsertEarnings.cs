using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162010)]
    public class InsertEarnings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Earning")
                .Row(new { Id = 1, Description = "Décimo Terceiro", Value = 2000, UserId = 1, Date = new DateTime(2020, 12, 20) })
                .Row(new { Id = 2, Description = "Bônus", Value = 500, UserId = 1, Date = new DateTime(2020, 6, 15) })
                .Row(new { Id = 3, Description = "Reembolso", Value = 150, UserId = 1, Date = new DateTime(2020, 4, 10) })
                .Row(new { Id = 12, Description = "Venda", Value = 1000, UserId = 1, Date = new DateTime(2020, 4, 1) })
                .Row(new { Id = 32, Description = "Prêmio", Value = 2000, UserId = 3, Date = new DateTime(2020, 12, 1) });
        }

        public override void Down()
        {
        }
    }
}
