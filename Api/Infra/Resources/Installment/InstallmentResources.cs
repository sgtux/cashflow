namespace Api.Infra.Resources.Payment
{
    public class InstallmentResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Installment.Delete.sql");

        public static ResourceBuilder Insert => new ResourceBuilder("Installment.Insert.sql");
    }
}