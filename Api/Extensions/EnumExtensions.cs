using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this ExpenseType e)
        {
            switch (e)
            {
                case ExpenseType.Food:
                    return "Alimentação";
                case ExpenseType.Market:
                    return "Mercado";
                case ExpenseType.Snack:
                    return "Lanche";
                case ExpenseType.Aesthetics:
                    return "Estética";
                case ExpenseType.Education:
                    return "Educação";
                case ExpenseType.Vehicle:
                    return "Veículo";
                case ExpenseType.Pets:
                    return "Pets";
                case ExpenseType.Leisure:
                    return "Lazer";
                case ExpenseType.HardDrink:
                    return "Bebida Alcoólica";
                case ExpenseType.Party:
                    return "Festa";
                case ExpenseType.Transport:
                    return "Transporte";
                case ExpenseType.LateFees:
                    return "Juros de Atraso";
                case ExpenseType.Clothes:
                    return "Roupas";
                case ExpenseType.Utilities:
                    return "Utilidades";
                case ExpenseType.Cleaning:
                    return "Limpeza";
                case ExpenseType.Exchange:
                    return "Câmbio";
                case ExpenseType.Investment:
                    return "Investimentos";
                case ExpenseType.Phone:
                    return "Telefone";
                case ExpenseType.Games:
                    return "Jogos";
                case ExpenseType.Donation:
                    return "Doação";
                case ExpenseType.Streams:
                    return "Streams";
                case ExpenseType.Others:
                    return "Outros";
                case ExpenseType.Loan:
                    return "Empréstimo";
                case ExpenseType.Financing:
                    return "Financiamento";
                case ExpenseType.Tech:
                    return "Tecnologia";
                default:
                    return "Desconhecido";
            }
        }

        public static string GetDescription(this EarningType e)
        {
            switch (e)
            {
                case EarningType.Monthy:
                    return "Recorrente";
                case EarningType.Normal:
                    return "Normal";
                default:
                    return "Desconhecido";
            }
        }
    }
}