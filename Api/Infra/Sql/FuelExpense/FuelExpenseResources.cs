namespace Cashflow.Api.Infra.Sql.Vehicle
{
    public static class FuelExpenseResources
    {
        public static ResourceBuilder ById => new ResourceBuilder("FuelExpense.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("FuelExpense.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("FuelExpense.Update.sql");

        public static ResourceBuilder Delete => new ResourceBuilder("FuelExpense.Delete.sql");
    }
}