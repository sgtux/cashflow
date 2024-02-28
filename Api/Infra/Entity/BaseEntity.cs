using System;
using System.Text.Json.Serialization;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Entity
{
    public abstract class BaseEntity
    {
        [JsonIgnore]
        public DateTime CurrentDate => Utils.CurrentDate;
    }
}