namespace Cashflow.Api.Infra.Sql.User
{
    public static class UserResources
    {
        public static ResourceBuilder ByNickName => new ResourceBuilder("User.GetByNickName.sql");

        public static ResourceBuilder ById => new ResourceBuilder("User.GetById.sql");

        public static ResourceBuilder Insert = new ResourceBuilder("User.Insert.sql");
    }
}