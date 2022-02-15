using System;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Infra.Entity
{
    public class HouseholdExpense : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public decimal Value { get; set; }

        public int? VehicleId { get; set; }

        public HouseholdExpenseType Type { get; set; }

        public string TypeDescription => ((HouseholdExpenseType)Type).GetDescription();

        public bool IsRecurrent => Type == HouseholdExpenseType.Food
        || Type == HouseholdExpenseType.Hygiene
        || Type == HouseholdExpenseType.Leisure
        || Type == HouseholdExpenseType.MarketRanch
        || Type == HouseholdExpenseType.Party
        || Type == HouseholdExpenseType.Pets;
    }
}