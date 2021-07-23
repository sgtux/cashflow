using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Extensions;
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

        public string ConditionText => Condition.GetDescription();

        public bool Monthly => Condition == PaymentConditionEnum.Monthly;

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public bool Invoice { get; set; }

        public string CreditCardText => CreditCard != null ? Invoice ? $"{CreditCard.Name} (Fatura)" : CreditCard.Name : "";

        public bool Paid { get; set; }

        public IList<Installment> Installments { get; set; }

        public int PaidInstallments => Installments?.Where(p => p.PaidDate.HasValue).Count() ?? 0;

        public decimal Total => Installments?.Sum(p => p.Cost) ?? 0;

        public string FirstPaymentFormatted => Installments?.OrderBy(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");
    }
}