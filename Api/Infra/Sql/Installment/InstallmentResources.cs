namespace Cashflow.Api.Infra.Sql.Payment
{
    public static class InstallmentResources
    {
        public static ResourceBuilder Delete => new("Installment.Delete.sql");

        public static ResourceBuilder Insert => new("Installment.Insert.sql");

        public static ResourceBuilder Update => new("Installment.Update.sql");
    }
}