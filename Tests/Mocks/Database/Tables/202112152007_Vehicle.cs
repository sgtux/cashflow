using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152007)]
    public class Vehicle : Migration
    {
        public override void Up()
        {
            Create.Table("Vehicle")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable();

            Execute.Sql("ALTER TABLE Vehicle ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}