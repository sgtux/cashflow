namespace Cashflow.Api.Models
{
    public class RemainingBalanceModel
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal Value { get; set; }
    }
}