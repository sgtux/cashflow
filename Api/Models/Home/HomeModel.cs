using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Models.Home
{
    public class HomeModel
    {
        public short Month { get; set; }

        public short Year { get; set; }

        public List<ChartModel> ChartInfos { get; set; }

        public List<PendingPaymentModel> PendingPayments { get; set; }

        public List<LimitValueModel> LimitValues { get; set; }

        public List<InflowOutflowModel> Inflows { get; set; }

        public List<InflowOutflowModel> Outflows { get; set; }

        public decimal TotalInflows => Inflows.Sum(p => p.Value);

        public decimal TotalOutflows => Outflows.Sum(p => p.Value);

        public HomeModel(short month, short year)
        {
            Month = month;
            Year = year;
            ChartInfos = new List<ChartModel>();
            PendingPayments = new List<PendingPaymentModel>();
            LimitValues = new List<LimitValueModel>();
            Inflows = new List<InflowOutflowModel>();
            Outflows = new List<InflowOutflowModel>();            
        }
    }
}