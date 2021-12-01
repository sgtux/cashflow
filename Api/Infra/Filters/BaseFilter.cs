using System;

namespace Cashflow.Api.Infra.Filters
{
    public class BaseFilter
    {
        public int UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}