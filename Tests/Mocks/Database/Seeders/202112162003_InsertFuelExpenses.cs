using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162003)]
    public class InsertFuelExpenses : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("FuelExpenses")
                .Row(new { Id = 1, Miliage = 100, PricePerLiter = 9.5, ValueSupplied = 30.5, VehicleId = 1, Date = new DateTime(2020, 10, 1) })
                .Row(new { Id = 2, Miliage = 100, PricePerLiter = 9.5, ValueSupplied = 30.5, VehicleId = 1, Date = new DateTime(2020, 10, 1) })
                .Row(new { Id = 3, Miliage = 150, PricePerLiter = 6.5, ValueSupplied = 30.5, VehicleId = 2, Date = new DateTime(2020, 11, 8) })
                .Row(new { Id = 4, Miliage = 250, PricePerLiter = 8.5, ValueSupplied = 40.5, VehicleId = 1, Date = new DateTime(2020, 12, 10) })
                .Row(new { Id = 5, Miliage = 200, PricePerLiter = 7.5, ValueSupplied = 50.5, VehicleId = 2, Date = new DateTime(2020, 12, 9) })
                .Row(new { Id = 6, Miliage = 100, PricePerLiter = 6.5, ValueSupplied = 50.5, VehicleId = 3, Date = new DateTime(2020, 12, 10) })
                .Row(new { Id = 7, Miliage = 150, PricePerLiter = 7.5, ValueSupplied = 50.5, VehicleId = 3, Date = new DateTime(2020, 12, 11) })
                .Row(new { Id = 8, Miliage = 200, PricePerLiter = 7.5, ValueSupplied = 50.5, VehicleId = 3, Date = new DateTime(2020, 12, 12) })
                .Row(new { Id = 9, Miliage = 200, PricePerLiter = 7.5, ValueSupplied = 100, VehicleId = 5, Date = DateTime.Now.AddMonths(-1) })
                .Row(new { Id = 10, Miliage = 200, PricePerLiter = 7.5, ValueSupplied = 200, VehicleId = 5, Date = DateTime.Now });
        }

        public override void Down()
        {
        }
    }
}