using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class Payment : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public PaymentType Type { get; set; }

        public PaymentTypeEnum TypeId { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public string CreditCardText => CreditCard?.Name ?? string.Empty;

        public DateTime Date { get; set; }

        public bool Done => Installments?.All(p => p.PaidDate.HasValue) ?? false;

        public bool PaidInThisMonth => Done && (Installments?.Any(p => p.PaidDate.HasValue && p.PaidDate.Value.SameMonthYear(DateTime.Now)) ?? false);

        public IList<Installment> Installments { get; set; }

        public int PaidInstallments => Installments?.Where(p => p.PaidDate.HasValue).Count() ?? 0;

        public decimal Total => Installments?.Sum(p => p.Cost) ?? 0;

        public string FirstPaymentFormatted => Installments?.OrderBy(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");
    }
}