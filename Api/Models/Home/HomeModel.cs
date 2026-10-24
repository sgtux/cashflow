using System;
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

        public decimal PendingPaymentsTotal => PendingPayments?.Sum(p => p.Value) ?? 0;

        public List<LimitValueModel> LimitValues { get; set; }

        public List<InflowOutflowModel> Inflows { get; set; }

        public List<InflowOutflowModel> Outflows { get; set; }

        public decimal TotalInflows => Inflows.Sum(p => p.Value);

        public decimal TotalOutflows => Outflows.Sum(p => p.Value);

        public decimal TotalPassiveInflows => Inflows.Where(p => p.IsPassive).Sum(p => p.Value);

        // Saídas do tipo Investimento (aportes): saem da conta, mas não são custo de vida.
        public decimal InvestmentOutflows { get; set; }

        public decimal TotalLivingCosts => TotalOutflows - InvestmentOutflows;

        // Quanto do custo de vida do mês a renda passiva cobre (1 = 100%). A partir de 1, saiu da corrida dos ratos.
        public decimal PassiveIncomeCoverage => TotalLivingCosts > 0 ? Math.Round(TotalPassiveInflows / TotalLivingCosts, 4) : 0;

        public bool OutOfRatRace => TotalLivingCosts > 0 && TotalPassiveInflows >= TotalLivingCosts;

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