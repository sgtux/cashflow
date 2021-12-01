namespace Cashflow.Api.Infra.Sql.DailyExpenses
{
    public static class DailyExpensesResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("DailyExpenses.Delete.sql");

        public static ResourceBuilder DeleteItems => new ResourceBuilder("DailyExpenses.DeleteItems.sql");

        public static ResourceBuilder Some => new ResourceBuilder("DailyExpenses.GetSome.sql");

        public static ResourceBuilder ById => new ResourceBuilder("DailyExpenses.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("DailyExpenses.Insert.sql");

        public static ResourceBuilder InsertItem => new ResourceBuilder("DailyExpenses.InsertItem.sql");

        public static ResourceBuilder Update => new ResourceBuilder("DailyExpenses.Update.sql");
    }
}