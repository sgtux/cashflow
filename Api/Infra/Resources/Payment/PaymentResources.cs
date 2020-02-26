namespace Api.Infra.Resources.Payment
{
    public class PaymentResources
    {
        public static ResourceBuilder Delete => new ResourceBuilder("Payment.Delete.sql");
        public static ResourceBuilder ById => new ResourceBuilder("Payment.GetById.sql");
        public static ResourceBuilder ByUser => new ResourceBuilder("Payment.GetByUser.sql");
        public static ResourceBuilder Insert => new ResourceBuilder("Payment.Insert.sql");
        public static ResourceBuilder Update => new ResourceBuilder("Payment.Update.sql");
    }
}