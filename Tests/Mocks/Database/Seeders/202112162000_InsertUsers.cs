using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162000)]
    public class InsertUsers : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("User")
                .Row(new { Id = 1, NickName = "User1", Password = "40BD001563085FC35165329EA1FF5C5ECBDBBEEF", CreatedAt = new DateTime(2018, 1, 1) })
                .Row(new { Id = 2, NickName = "User2", Password = "40BD001563085FC35165329EA1FF5C5ECBDBBEEF", CreatedAt = new DateTime(2018, 1, 1) })
                .Row(new { Id = 3, NickName = "User3", Password = "40BD001563085FC35165329EA1FF5C5ECBDBBEEF", CreatedAt = new DateTime(2018, 1, 1) });
        }

        public override void Down()
        {
        }
    }
}