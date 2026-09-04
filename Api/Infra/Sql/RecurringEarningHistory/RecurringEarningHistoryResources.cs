namespace Cashflow.Api.Infra.Sql.RecurringEarning
{
    public static class RecurringEarningHistoryResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("RecurringEarningHistory.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("RecurringEarningHistory.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("RecurringEarningHistory.Update.sql");
    }
}
