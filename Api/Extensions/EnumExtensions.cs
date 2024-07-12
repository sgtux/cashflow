using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this PaymentType type)
        {
            switch (type)
            {
                case PaymentType.Spending:
                    return "Gastos";
                case PaymentType.Financing:
                    return "Financiamento";
                case PaymentType.Education:
                    return "Educação";
                case PaymentType.Loan:
                    return "Empréstimo";
                case PaymentType.Donation:
                    return "Doação";
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
                case HouseholdExpenseType.Market:
                    return "Mercado";
                case HouseholdExpenseType.Snack:
                    return "Lanche";
                case HouseholdExpenseType.Aesthetics:
                    return "Estética";
                case HouseholdExpenseType.Education:
                    return "Educação";
                case HouseholdExpenseType.Vehicle:
                    return "Veículo";
                case HouseholdExpenseType.Pets:
                    return "Pets";
                case HouseholdExpenseType.Leisure:
                    return "Lazer";
                case HouseholdExpenseType.HardDrink:
                    return "Bebida Alcoólica";
                case HouseholdExpenseType.Party:
                    return "Festa";
                case HouseholdExpenseType.Transport:
                    return "Transporte";
                case HouseholdExpenseType.LateFees:
                    return "Juros de Atraso";
                case HouseholdExpenseType.Clothes:
                    return "Roupas";
                case HouseholdExpenseType.Utilities:
                    return "Utilidades";
                case HouseholdExpenseType.Cleaning:
                    return "Limpeza";
                case HouseholdExpenseType.Exchange:
                    return "Câmbio";
                case HouseholdExpenseType.Investment:
                    return "Investimentos";
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
                case EarningType.Monthy:
                    return "Mensal";
                case EarningType.Normal:
                    return "Normal";
                default:
                    return "Desconhecido";
            }
        }
    }
}