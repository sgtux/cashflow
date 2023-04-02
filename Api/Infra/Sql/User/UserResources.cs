namespace Cashflow.Api.Infra.Sql.User
{
    public static class UserResources
    {
        public static ResourceBuilder ByEmail => new ResourceBuilder("User.GetByEmail.sql");

        public static ResourceBuilder ById => new ResourceBuilder("User.GetById.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("User.Insert.sql");

        public static ResourceBuilder UpdateSpendingCeiling => new ResourceBuilder("User.UpdateSpendingCeiling.sql");
    }
}