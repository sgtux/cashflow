namespace Cashflow.Api.Infra.Entity
{
    public class RemainingBalance
    {
        public long Id { get; set; }

        public decimal Value { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int UserId { get; set; }
    }
}