namespace Cashflow.Api.Infra.Sql.RecurringExpense
{
    public static class RecurringExpenseResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("RecurringExpense.Delete.sql");

        public static ResourceBuilder ById => new ResourceBuilder("RecurringExpense.GetById.sql");

        public static ResourceBuilder Some => new ResourceBuilder("RecurringExpense.GetSome.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("RecurringExpense.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("RecurringExpense.Update.sql");
    }
}