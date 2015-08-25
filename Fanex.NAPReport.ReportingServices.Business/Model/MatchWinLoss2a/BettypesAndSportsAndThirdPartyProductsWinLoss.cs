// -----------------------------------------------------------------------
// <copyright file="BettypesAndSportsAndThirdPartyProductsWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BettypesAndSportsAndThirdPartyProductsWinLoss
    {
        public BettypesAndSportsAndThirdPartyProductsWinLoss()
        {
            SummaryBettypesWinLoss = new SummaryBettypesWinLoss()
                {
                    GrandTotal = new SummaryBettypesWinLoss()
                };
            SportsAndThirdPartyProductsWinLoss = new SportsAndThirdPartyProductsWinLoss();
        }

        public SummaryBettypesWinLoss SummaryBettypesWinLoss { get; set; }

        public SportsAndThirdPartyProductsWinLoss SportsAndThirdPartyProductsWinLoss { get; set; }
    }
}