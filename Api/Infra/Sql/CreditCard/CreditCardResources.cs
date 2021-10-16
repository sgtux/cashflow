namespace Cashflow.Api.Infra.Sql.CreditCard
{
    public static class CreditCardResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("CreditCard.Delete.sql");

        public static ResourceBuilder ByUser => new ResourceBuilder("CreditCard.GetByUser.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("CreditCard.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("CreditCard.Update.sql");

        public static ResourceBuilder HasPayments => new ResourceBuilder("CreditCard.HasPayments.sql");
    }
}