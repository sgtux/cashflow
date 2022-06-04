using Cashflow.Api.Enums;

namespace Cashflow.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this PaymentCondition conditionEnum)
        {
            switch (conditionEnum)
            {
                case PaymentCondition.Cash:
                    return "À Vista";
                case PaymentCondition.Monthly:
                    return "Mensal";
                case PaymentCondition.Installment:
                    return "Parcelado";
                default:
                    return "Desconhecido";
            }
        }

        public static string GetDescription(this HouseholdExpenseType e)
        {
            switch (e)
            {
                case HouseholdExpenseType.Food:
                    return "Comida";
                case HouseholdExpenseType.MarketRanch:
                    return "Rancho";
                case HouseholdExpenseType.FastFood:
                    return "Fast-Food";
                case HouseholdExpenseType.Hygiene:
                    return "Higiene";
                case HouseholdExpenseType.Education:
                    return "Educação";
                case HouseholdExpenseType.Vehicle:
                    return "Veículo";
                case HouseholdExpenseType.Pets:
                    return "Pets";
                case HouseholdExpenseType.Leisure:
                    return "Lazer";
                case HouseholdExpenseType.HardDrink:
                    return "Bebida Alcólica";
                case HouseholdExpenseType.Party:
                    return "Festa";
                case HouseholdExpenseType.Uber:
                    return "Uber";
                case HouseholdExpenseType.DelayInterest:
                    return "Juros de Atraso";
                case HouseholdExpenseType.Others:
                    return "Outros";
                default:
                    return "Desconhecido";
            }
        }

        public static string GetDescription(this EarningType e)
        {
            switch (e)
            {
                case EarningType.MonthyBenefit:
                    return "Benefício Mensal";
                case EarningType.Benefit:
                    return "Benefício";
                default:
                    return "Desconhecido";
            }
        }
    }
}