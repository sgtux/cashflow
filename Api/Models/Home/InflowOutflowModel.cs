namespace Cashflow.Api.Models.Home
{
    public class InflowOutflowModel
    {
        public string Description { get; set; }

        public decimal Value { get; set; }

        public bool IsPassive { get; set; }

        public InflowOutflowModel() { }

        public InflowOutflowModel(string description, decimal value)
        {
            Description = description;
            Value = value;
        }

        public InflowOutflowModel(string description, decimal value, bool isPassive) : this(description, value) => IsPassive = isPassive;

        public InflowOutflowModel(string description) => Description = description;
    }
}