namespace Cashflow.Api.Infra.Sql.SystemParameter
{
    public static class SystemParameterResources
    {
        public static ResourceBuilder ByKey => new ResourceBuilder("SystemParameter.GetByKey.sql");
    }
}