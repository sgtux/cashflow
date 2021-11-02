using System.Text.Json.Serialization;

namespace Cashflow.Api.Infra.Entity
{
    public class DailyExpensesItem
    {
        public long Id { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public decimal TotalPrice => Amount * Price;

        [JsonIgnore]
        public long DailyExpensesId { get; set; }
    }
}