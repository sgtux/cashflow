using System;

namespace Cashflow.Api.Infra.Entity
{
    public class HouseholdExpense
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public decimal Value { get; set; }
    }
}