using System;

namespace Cashflow.Api.Infra.Entity
{
    public class FuelExpenses : BaseEntity
    {
        public int Id { get; set; }

        public int Miliage { get; set; }

        public decimal ValueSupplied { get; set; }

        public decimal PricePerLiter { get; set; }

        public DateTime Date { get; set; }

        public int VehicleId { get; set; }

        public decimal LitersSupplied => ValueSupplied == 0 || PricePerLiter == 0 ? 0 : Math.Round(ValueSupplied / PricePerLiter, 1);

        public int? CreditCardId { get; set; }

        public bool HasCreditCard => CreditCardId.HasValue;

        public CreditCard CreditCard { get; set; }

        public string CreditCardText => CreditCard?.Name;

        public bool CurrentInvoice => CreditCard != null && Date.Day <= CreditCard.InvoiceClosingDay;

        public bool NextInvoice => CreditCard != null && Date.Day > CreditCard.InvoiceClosingDay;
    }
}