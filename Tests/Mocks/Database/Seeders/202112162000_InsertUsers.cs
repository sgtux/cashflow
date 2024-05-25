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
                .Row(new { Id = 1, Email = "User1@mail.com", Password = "$2a$12$Y1QWzCGPDiHmVGWaClQDIO.eh4nzGqn6UBXZgfOePCb6T0ubi5Ja6", CreatedAt = new DateTime(2018, 1, 1) })
                .Row(new { Id = 2, Email = "User2@mail.com", Password = "$2a$12$Y1QWzCGPDiHmVGWaClQDIO.eh4nzGqn6UBXZgfOePCb6T0ubi5Ja6", CreatedAt = new DateTime(2018, 1, 1) })
                .Row(new { Id = 3, Email = "User3@mail.com", Password = "$2a$12$Y1QWzCGPDiHmVGWaClQDIO.eh4nzGqn6UBXZgfOePCb6T0ubi5Ja6", CreatedAt = new DateTime(2018, 1, 1) })
                .Row(new { Id = 4, Email = "User4@mail.com", Password = "$2a$12$Y1QWzCGPDiHmVGWaClQDIO.eh4nzGqn6UBXZgfOePCb6T0ubi5Ja6", CreatedAt = new DateTime(2018, 1, 1) });
        }

        public override void Down()
        {
        }
    }
}