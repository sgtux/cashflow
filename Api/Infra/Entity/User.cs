using System;

namespace Cashflow.Api.Infra.Entity
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public decimal SpendingCeiling { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}