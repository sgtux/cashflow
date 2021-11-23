namespace Cashflow.Api.Infra.Sql.Vehicle
{
    public static class FuelExpensesResources
    {
        public static ResourceBuilder ById => new ResourceBuilder("FuelExpenses.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("FuelExpenses.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("FuelExpenses.Update.sql");

        public static ResourceBuilder Delete => new ResourceBuilder("FuelExpenses.Delete.sql");
    }
}