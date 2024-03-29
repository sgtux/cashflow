using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Infra.Entity
{
    public class Vehicle : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public List<FuelExpense> FuelExpenses { get; set; }

        public decimal MiliageTraveled => HasExpenses ? FuelExpenses.Max(p => p.Miliage) - FuelExpenses.Min(p => p.Miliage) : 0;

        public decimal MiliagePerLiter
        {
            get
            {
                if (!HasExpenses)
                    return 0;

                var sum = FuelExpenses.Sum(p => p.LitersSupplied) - FuelExpenses.OrderByDescending(p => p.Date).First().LitersSupplied;
                return Math.Round(MiliageTraveled / sum, 2);
            }
        }

        private bool HasExpenses => FuelExpenses?.Count > 1;
    }
}