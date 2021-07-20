using System.Collections.Generic;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Entity
{
    public class Payment
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public PaymentType Type { get; set; }

        public PaymentTypeEnum TypeId { get; set; }

        public PaymentConditionEnum Condition { get; set; }

        public int? CreditCardId { get; set; }

        public bool Invoice { get; set; }

        public bool Paid { get; set; }

        public IList<Installment> Installments { get; set; }

        public bool Monthly => Condition == PaymentConditionEnum.Monthly;
    }
}