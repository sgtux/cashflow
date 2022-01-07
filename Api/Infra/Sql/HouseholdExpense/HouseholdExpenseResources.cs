namespace Cashflow.Api.Infra.Sql.HouseholdExpense
{
    public static class HouseholdExpenseResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("HouseholdExpense.Delete.sql");

        public static ResourceBuilder DeleteItems => new ResourceBuilder("HouseholdExpense.DeleteItems.sql");

        public static ResourceBuilder Some => new ResourceBuilder("HouseholdExpense.GetSome.sql");

        public static ResourceBuilder ById => new ResourceBuilder("HouseholdExpense.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("HouseholdExpense.Insert.sql");

        public static ResourceBuilder InsertItem => new ResourceBuilder("HouseholdExpense.InsertItem.sql");

        public static ResourceBuilder Update => new ResourceBuilder("HouseholdExpense.Update.sql");
    }
}