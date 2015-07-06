// -----------------------------------------------------------------------
// <copyright file="SportsAndThirdPartyProductsWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections;

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
    }
}