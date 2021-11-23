using System.Collections.Generic;

namespace Cashflow.Api.Infra.Entity
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public List<FuelExpenses> FuelExpenses { get; set; }
    }
}