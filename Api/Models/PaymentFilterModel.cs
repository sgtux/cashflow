using System;

namespace Cashflow.Api.Models
{
    public class PaymentFilterModel
    {
        public string Description { get; set; }

        public DateTime? InactiveFrom { get; set; }

        public DateTime? InactiveTo { get; set; }

        public bool? Active { get; set; }
    }
}