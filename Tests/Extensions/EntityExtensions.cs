using Cashflow.Api.Infra.Entity;

namespace Cashflow.Tests.Extensions
{
    public static class EntityExtensions
    {
        public static object ToSqliteEntity(this InstallmentEntity installment)
        {
            return new
            {
                installment.Id,
                installment.PaymentId,
                installment.Number,
                installment.Value,
                installment.Date,
                installment.PaidDate,
                installment.PaidValue,
                installment.Exempt
            };
        }
    }
}