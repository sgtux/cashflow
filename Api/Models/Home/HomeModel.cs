using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Models.Home
{
    public class HomeModel
    {
        public List<ChartModel> ChartInfos { get; set; }

        public List<PendingPaymentModel> PendingPayments { get; set; }

        public short? Month => ChartInfos?.FirstOrDefault()?.Month;

        public short? Year => ChartInfos?.FirstOrDefault()?.Year;

        public HomeModel()
        {
            ChartInfos = new List<ChartModel>();
            PendingPayments = new List<PendingPaymentModel>();
        }
    }
}