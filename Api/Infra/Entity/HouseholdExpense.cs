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

        public HouseholdExpenseTypeEnum Type { get; set; }

        public string TypeDescription => ((HouseholdExpenseTypeEnum)Type).GetDescription();
    }
}