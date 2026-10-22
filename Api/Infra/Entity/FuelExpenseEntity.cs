using System;

namespace Cashflow.Api.Infra.Entity
{
    public class FuelExpenseEntity : BaseEntity
    {
        public int Id { get; set; }

        public int Miliage { get; set; }

        public decimal ValueSupplied { get; set; }

        public decimal PricePerLiter { get; set; }

        public DateTime Date { get; set; }

        public int VehicleId { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCardEntity CreditCard { get; set; }

        public string CreditCardName => CreditCard?.Name;

        public DateTime InvoiceDate => CreditCard == null || Date.Day < CreditCard.InvoiceClosingDay ? Date : Date.AddMonths(1);

        public decimal LitersSupplied => ValueSupplied == 0 || PricePerLiter == 0 ? 0 : Math.Round(ValueSupplied / PricePerLiter, 1);
    }
}