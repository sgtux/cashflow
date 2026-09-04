namespace Cashflow.Api.Infra.Sql.RecurringEarning
{
    public static class RecurringEarningResources
    {
        public static ResourceBuilder Inactivate => new ResourceBuilder("RecurringEarning.Inactivate.sql");

        public static ResourceBuilder ById => new ResourceBuilder("RecurringEarning.GetById.sql");

        public static ResourceBuilder Some => new ResourceBuilder("RecurringEarning.GetSome.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("RecurringEarning.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("RecurringEarning.Update.sql");
    }
}
