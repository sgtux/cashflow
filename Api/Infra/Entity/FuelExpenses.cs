namespace Cashflow.Api.Infra.Entity
{
    public class FuelExpenses
    {
        public int Id { get; set; }

        public int Miliage { get; set; }

        public decimal ValueSupplied { get; set; }

        public decimal PricePerLiter { get; set; }

        public int VehicleId { get; set; }
    }
}