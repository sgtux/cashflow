using System;

namespace Cashflow.Api.Infra.Filters
{
    public class BaseFilter
    {
        public int UserId { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? InactiveFrom { get; set; }

        public DateTime? InactiveTo { get; set; }

        public bool? Active { get; set; }
    }
}