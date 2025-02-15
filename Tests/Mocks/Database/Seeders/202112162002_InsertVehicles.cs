using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162002)]
    public class InsertVehicles : Migration
    {

        public override void Up()
        {
            Insert.IntoTable("Vehicle")
                .Row(new { Id = 1, Description = "Moto User 1", UserId = 1, Active = true })
                .Row(new { Id = 2, Description = "Carro User 1", UserId = 1, Active = true })
                .Row(new { Id = 3, Description = "Moto User 2", UserId = 2, Active = true })
                .Row(new { Id = 4, Description = "Carro User 2", UserId = 2, Active = true })
                .Row(new { Id = 5, Description = "Carro User 4", UserId = 4, Active = true });
        }

        public override void Down()
        {
        }
    }
}