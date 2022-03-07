using System;
namespace Cashflow.Api.Models
{
    public class PaymentFilterModel
    {
        public string Description { get; set; }

        public bool? Done { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}