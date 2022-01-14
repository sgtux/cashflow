namespace Cashflow.Api.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this PaymentConditionEnum conditionEnum)
        {
            switch (conditionEnum)
            {
                case PaymentConditionEnum.Cash:
                    return "À Vista";
                case PaymentConditionEnum.Monthly:
                    return "Mensal";
                case PaymentConditionEnum.Installment:
                    return "Parcelado";
                default:
                    return "Condição do Pagamento";
            }
        }

        public static string GetDescription(this HouseholdExpenseTypeEnum e)
        {
            switch (e)
            {
                case HouseholdExpenseTypeEnum.Food:
                    return "Comida";
                case HouseholdExpenseTypeEnum.MarketRanch:
                    return "Rancho";
                case HouseholdExpenseTypeEnum.FastFood:
                    return "Fast-Food";
                case HouseholdExpenseTypeEnum.Hygiene:
                    return "Higiene";
                case HouseholdExpenseTypeEnum.Education:
                    return "Educação";
                case HouseholdExpenseTypeEnum.Vehicle:
                    return "Veículo";
                case HouseholdExpenseTypeEnum.Pets:
                    return "Animal";
                case HouseholdExpenseTypeEnum.Leisure:
                    return "Lazer";
                case HouseholdExpenseTypeEnum.HardDrink:
                    return "Bebida Alcólica";
                case HouseholdExpenseTypeEnum.Party:
                    return "Festa";
                case HouseholdExpenseTypeEnum.Others:
                    return "Outros";
                default:
                    return "Tipo";
            }
        }
    }
}