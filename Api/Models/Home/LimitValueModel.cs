namespace Cashflow.Api.Models.Home
{
    public class LimitValueModel
    {
        public string Description { get; set; }

        public decimal Limit { get; set; }

        public decimal Spent { get; set; }
    }
}