namespace Cashflow.Tests.TestModels
{
    public class ApiResultDataModel<T> : ApiResultModel
    {
        public T Data { get; set; }
    }
}