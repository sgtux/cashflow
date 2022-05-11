using Cashflow.Api.Infra.Entity;

namespace Cashflow.Tests.Extensions
{
    public static class EntityExtensions
    {
        public static object ToSqliteEntity(this Installment installment)
        {
            return new
            {
                Id = installment.Id,
                PaymentId = installment.PaymentId,
                Number = installment.Number,
                Value = installment.Value,
                Date = installment.Date,
                PaidDate = installment.PaidDate,
                PaidValue = installment.PaidValue,
                Exempt = installment.Exempt
            };
        }
    }
}