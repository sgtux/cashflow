using System;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

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

        public string TypeDescription => Type.GetDescription();

        public bool IsRecurrent => Type != HouseholdExpenseType.LateFees && Type != HouseholdExpenseType.Others;
    }
}