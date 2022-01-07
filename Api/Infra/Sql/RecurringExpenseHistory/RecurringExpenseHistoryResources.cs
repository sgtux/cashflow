namespace Cashflow.Api.Infra.Sql.RecurringExpense
{
    public static class RecurringExpenseHistoryResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("RecurringExpenseHistory.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("RecurringExpenseHistory.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("RecurringExpenseHistory.Update.sql");
    }
}