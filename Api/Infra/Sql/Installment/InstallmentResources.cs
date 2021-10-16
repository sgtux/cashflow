namespace Cashflow.Api.Infra.Sql.Payment
{
    public class InstallmentResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Installment.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Installment.Insert.sql");
    }
}