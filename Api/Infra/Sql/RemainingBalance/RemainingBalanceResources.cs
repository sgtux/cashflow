namespace Cashflow.Api.Infra.Sql.CreditCard
{
    public static class RemainingBalanceResources
    {
        public static ResourceBuilder ByMonthYear => new ResourceBuilder("RemainingBalance.GetByMonthYear.sql");

        public static ResourceBuilder Some => new ResourceBuilder("RemainingBalance.GetSome.sql");

        public static ResourceBuilder Delete => new ResourceBuilder("RemainingBalance.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("RemainingBalance.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("RemainingBalance.Update.sql");
    }
}