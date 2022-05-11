using System;

namespace Cashflow.Api.Infra.Entity
{
    public class Installment : BaseEntity
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public short Number { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public decimal? PaidValue { get; set; }

        public DateTime? PaidDate { get; set; }

        public bool Exempt { get; set; }
    }
}