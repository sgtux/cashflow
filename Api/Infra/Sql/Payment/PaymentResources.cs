namespace Cashflow.Api.Infra.Sql.Payment
{
    public static class PaymentResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Payment.Delete.sql");

        public static ResourceBuilder ById => new ResourceBuilder("Payment.GetById.sql");

        public static ResourceBuilder Some => new ResourceBuilder("Payment.GetSome.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Payment.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("Payment.Update.sql");
    }
}