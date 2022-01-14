using Cashflow.Api.Enums;

namespace Cashflow.Api.Models.HouseholdExpense
{
    public class HouseholdExpenseTypeModel
    {
        private HouseholdExpenseTypeEnum _type;

        public HouseholdExpenseTypeModel(HouseholdExpenseTypeEnum e)
        {
            _type = e;
        }

        public byte Id => (byte)_type;

        public string Description => _type.GetDescription();
    }
}