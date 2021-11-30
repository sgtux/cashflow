using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Infra.Entity
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public List<FuelExpenses> FuelExpenses { get; set; }

        public decimal MiliageTraveled => FuelExpenses.Any() ? FuelExpenses.Max(p => p.Miliage) - FuelExpenses.Min(p => p.Miliage) : 0;

        public decimal MilagePerLiter => FuelExpenses.Any() ? Math.Round(MiliageTraveled / FuelExpenses.Sum(p => p.LitersSupplied), 2) : 0;
    }
}