using System;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class Earning : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public EarningType Type { get; set; }

        public string TypeDescription => Type.GetDescription();
    }
}