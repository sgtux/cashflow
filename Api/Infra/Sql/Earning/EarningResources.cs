namespace Cashflow.Api.Infra.Sql.Earning
{
    public static class EarningResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Earning.Delete.sql");

        public static ResourceBuilder ById => new ResourceBuilder("Earning.GetById.sql");

        public static ResourceBuilder ByUser => new ResourceBuilder("Earning.GetSome.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Earning.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("Earning.Update.sql");
    }
}