using System.Text.Json.Serialization;

namespace Cashflow.Api.Infra.Entity
{
    public class DailyExpensesItem
    {
        public string ItemName { get; set; }

        public decimal Price { get; set; }

        [JsonIgnore]
        public long DailyExpensesId { get; set; }
    }
}