namespace Cashflow.Api.Infra.Entity
{
    public class SystemParameterEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }
    }
}