using System;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class Salary : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Value { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public void SetDays()
        {
            StartDate = StartDate.FixFirstDayInMonth();
            EndDate = EndDate?.FixLastDayInMonth();
        }
    }
}