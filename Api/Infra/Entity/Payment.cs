using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Infra.Entity
{
    public class Payment : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public PaymentType Type { get; set; }

        public PaymentTypeEnum TypeId { get; set; }

        public PaymentCondition Condition { get; set; }

        public decimal BaseCost { get; set; }

        public string ConditionText => Condition.GetDescription();

        public bool Monthly => Condition == PaymentCondition.Monthly;

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public string CreditCardText => CreditCard?.Name ?? string.Empty;

        public DateTime? InactiveAt { get; set; }

        public DateTime Date { get; set; }

        public bool Active => !InactiveAt.HasValue;

        public IList<Installment> Installments { get; set; }

        public int PaidInstallments => Installments?.Where(p => p.PaidDate.HasValue).Count() ?? 0;

        public decimal Total => Monthly ? BaseCost : Installments?.Sum(p => p.Cost) ?? 0;

        public string FirstPaymentFormatted => Installments?.OrderBy(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");
    }
}