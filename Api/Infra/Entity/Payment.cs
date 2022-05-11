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

        public bool DoneInThisMonth => HasInstallments && Done && (Installments?.Max(p => p.PaidDate.Value).SameMonthYear(DateTime.Now) ?? false);

        public IList<Installment> Installments { get; set; }

        public int PaidInstallments => Installments?.Where(p => p.PaidDate.HasValue).Count() ?? 0;

        public decimal Total => Installments?.Sum(p => p.Value) ?? 0;

        public decimal TotalPaid => Installments?.Sum(p => p.PaidValue) ?? 0;

        public decimal InstallmentValue => Installments?.FirstOrDefault(p => p.Date.SameMonthYear(DateTime.Now))?.Value ?? Installments?.FirstOrDefault()?.Value ?? 0;

        public string FirstPaymentDate => Installments?.OrderBy(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");

        public string LastPaymentDate => Installments?.OrderByDescending(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");

        public bool CurrentMonthPaid => Installments?.FirstOrDefault(p => p.Date.SameMonthYear(DateTime.Now))?.PaidDate.HasValue ?? false;

        private bool HasInstallments => Installments?.Any() ?? false;
    }
}