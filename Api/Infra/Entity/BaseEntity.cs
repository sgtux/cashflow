using System;
using System.Text.Json.Serialization;

namespace Cashflow.Api.Infra.Entity
{
    public abstract class BaseEntity
    {
        [JsonIgnore]
        public DateTime CurrentDate => DateTime.Now;
    }
}