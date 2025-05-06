using System;
using Cashflow.Api.Extensions;
using Cashflow.Api.Utils;

namespace Cashflow.Api.Infra.Filters
{
    public class BaseFilter
    {
        protected DateTime CurrentDate => DateTimeUtils.CurrentDate;

        public BaseFilter() { }

        public BaseFilter(BaseFilter filter)
        {
            UserId = filter.UserId;
            Description = filter.Description;
            StartDate = filter.StartDate;
            EndDate = filter.EndDate;
            InactiveFrom = filter.InactiveFrom;
            InactiveTo = filter.InactiveTo;
            Active = filter.Active;
        }

        public int UserId { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? InactiveFrom { get; set; }

        public DateTime? InactiveTo { get; set; }

        public byte? Active { get; set; }

        public virtual void FixParams()
        {
            Description = Description.FormatToLike();
            StartDate = StartDate.FixStartTimeFilter();
            EndDate = StartDate.FixEndTimeFilter();
        }
    }
}