namespace Cashflow.Api.Infra.Sql.Vehicle
{
    public static class VehicleResources
    {
        public static ResourceBuilder ById => new ResourceBuilder("Vehicle.GetById.sql");

        public static ResourceBuilder ByUser => new ResourceBuilder("Vehicle.GetByUser.sql");

        public static ResourceBuilder Delete => new ResourceBuilder("Vehicle.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Vehicle.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("Vehicle.Update.sql");
    }
}