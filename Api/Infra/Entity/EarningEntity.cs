using System;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class EarningEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public EarningType Type { get; set; } = EarningType.Others;

        public string TypeDescription => Type.GetDescription();

        public bool IsPassive => Type.IsPassive();

        public int UserId { get; set; }
    }
}
