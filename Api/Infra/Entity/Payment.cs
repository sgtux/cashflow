using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Entity
{
    public class Payment : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public PaymentType Type { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public string CreditCardText => CreditCard?.Name ?? string.Empty;

        public DateTime Date { get; set; }

        public bool Done => Installments?.All(p => p.PaidDate.HasValue || p.Exempt) ?? false;

        public bool DoneInThisMonth => HasInstallments && Done && (Installments?.Where(p => p.PaidDate.HasValue).Max(p => p.PaidDate.Value).SameMonthYear(Utils.CurrentDate) ?? false);

        public IList<Installment> Installments { get; set; }

        public int PaidInstallments => Installments?.Count(p => p.PaidDate.HasValue || p.Exempt) ?? 0;

        public decimal Total => Installments?.Sum(p => p.Value) ?? 0;

        public decimal TotalPaid => Installments?.Sum(p => p.PaidValue) ?? 0;

        public decimal InstallmentValue => Installments?.FirstOrDefault(p => p.Date.SameMonthYear(Utils.CurrentDate))?.Value ?? Installments?.FirstOrDefault()?.Value ?? 0;

        public string FirstPaymentDate => Installments?.OrderBy(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");

        public string LastPaymentDate => Installments?.OrderByDescending(p => p.Number).FirstOrDefault()?.Date.ToString("dd/MM/yyyy");

        public bool CurrentMonthPaid => Installments?.FirstOrDefault(p => p.Date.SameMonthYear(Utils.CurrentDate))?.PaidDate.HasValue ?? false;

        private bool HasInstallments => Installments?.Any() ?? false;
    }
}