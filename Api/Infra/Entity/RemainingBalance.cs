using System;

namespace Cashflow.Api.Infra.Entity
{
    public class RemainingBalance : BaseEntity
    {
        public long Id { get; set; }

        public decimal Value { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int UserId { get; set; }

        public string MonthYearText => Date.ToString("MM/yyyy");

        public DateTime Date => new DateTime(Year, Month, 1);
    }
}