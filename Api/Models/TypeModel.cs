using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Models
{
    public class TypeModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public TypeModel() { }

        public TypeModel(HouseholdExpenseType type)
        {
            Id = (int)type;
            Description = type.GetDescription();
        }
    }
}