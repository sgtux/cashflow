namespace Cashflow.Api.Models.Home
{
    public class ChartModel
    {
        public int Index { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public short Month { get; set; }

        public short Year { get; set; }
    }
}