using System;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Infra.Entity
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public decimal ExpenseLimit { get; set; }

        public decimal FuelExpenseLimit { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserPlanType Plan { get; set; }

        public int RecordsUsed { get; set; }
    }
}