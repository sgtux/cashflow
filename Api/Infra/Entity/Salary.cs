using System;

namespace Cashflow.Api.Infra.Entity
{
    public class Salary
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Value { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public void SetDays()
        {
            StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
            if (EndDate != null)
            {
                var lastDayEnd = DateTime.DaysInMonth(EndDate.Value.Year, EndDate.Value.Month);
                EndDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, lastDayEnd);
            }
        }
    }
}