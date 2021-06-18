namespace Api.Infra.Resources.Salary
{
    public static class SalaryResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Salary.Delete.sql");

        public static ResourceBuilder ById => new ResourceBuilder("Salary.GetById.sql");

        public static ResourceBuilder ByUser => new ResourceBuilder("Salary.GetByUser.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Salary.Insert.sql");

        public static ResourceBuilder Update => new ResourceBuilder("Salary.Update.sql");
    }
}