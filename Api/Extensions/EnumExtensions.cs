using Cashflow.Api.Shared;

namespace Cashflow.Api.Extensions
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
    }
}