// -----------------------------------------------------------------------
// <copyright file="SportsAndThirdPartyProductsWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections;
    using Fanex.NAPReport.ReportingServices.Business.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SportsAndThirdPartyProductsWinLoss
    {
        public SportsAndThirdPartyProductsWinLoss()
        {
            SportsWinLoss = new SportsWinLoss();
            ThirdPartyProductsWinLoss = new SummarySportsWinLoss()
                {
                    GrandTotal = new SummarySportsWinLoss()
                };
        }

        public SportsWinLoss SportsWinLoss { get; set; }

        public SummarySportsWinLoss ThirdPartyProductsWinLoss { get; set; }

        public SummarySportsWinLoss GrandTotal
        { 
            get
            {
                return new SummarySportsWinLoss 
                {
                    Commission = this.SportsWinLoss.GrandTotal.Commission + this.ThirdPartyProductsWinLoss.GrandTotal.Commission,
                    VirtualCommission = this.SportsWinLoss.GrandTotal.VirtualCommission + this.ThirdPartyProductsWinLoss.GrandTotal.VirtualCommission,
                    LineTotal = this.SportsWinLoss.GrandTotal.LineTotal + this.ThirdPartyProductsWinLoss.GrandTotal.LineTotal,
                    VirtualLineTotal = this.SportsWinLoss.GrandTotal.VirtualLineTotal + this.ThirdPartyProductsWinLoss.GrandTotal.VirtualLineTotal,                  
                    IBCTurnover = this.SportsWinLoss.GrandTotal.Turnover + this.ThirdPartyProductsWinLoss.GrandTotal.IBCTurnover
                };
            }
        }
    }
}