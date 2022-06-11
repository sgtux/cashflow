using System;

namespace Cashflow.Api.Models
{
    public class GenerateInstallmentsModel
    {
        public decimal Value { get; set; }

        public int Amount { get; set; }

        public DateTime Date { get; set; }

        public int CreditCardId { get; set; }

        public bool ValueByInstallment { get; set; }
    }
}