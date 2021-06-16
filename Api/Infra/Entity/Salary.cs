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
    }
}