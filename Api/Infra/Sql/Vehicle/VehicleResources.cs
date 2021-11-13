namespace Cashflow.Api.Infra.Sql.Vehicle
{
    public static class VehicleResources
    {
        public static ResourceBuilder ById => new ResourceBuilder("Vehicle.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Vehicle.Insert.sql");
    }
}