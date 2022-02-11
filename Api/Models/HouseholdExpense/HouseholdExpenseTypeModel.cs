using Cashflow.Api.Enums;

namespace Cashflow.Api.Models.HouseholdExpense
{
    public class HouseholdExpenseTypeModel
    {
        private HouseholdExpenseType _type;

        public HouseholdExpenseTypeModel(HouseholdExpenseType e)
        {
            _type = e;
        }

        public byte Id => (byte)_type;

        public string Description => _type.GetDescription();
    }
}