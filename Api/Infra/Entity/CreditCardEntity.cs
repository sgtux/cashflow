using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Models.CreditCard;

namespace Cashflow.Api.Infra.Entity
{
    public class CreditCardEntity : BaseEntity
    {
        public CreditCardEntity() => Items = new();

        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public int InvoiceClosingDay { get; set; }

        public int InvoiceDueDay { get; set; }

        public List<CreditCardItemModel> Items { get; set; }

        public decimal OutstandingDebtTotal => Items.Sum(p => p.OutstandingDebt);

        public decimal CurrentMonthDebt => Items.Sum(p => p.CurrentMonthDebt);

        public bool IsCurrentMonthDebtPaid => Items.All(p => p.IsCurrentMonthDebtPaid);
    }
}