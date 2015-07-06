// -----------------------------------------------------------------------
// <copyright file="BettypesWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BettypeWinLoss
    {
        public BettypeWinLoss()
        {
            MatchWinLoss = new List<MatchWinLoss>();
            BettypesWinLossTotal = new MatchWinLoss();
        }

        public string BettypeName { get; set; }

        public bool LiveIndicator { get; set; }

        public List<MatchWinLoss> MatchWinLoss { get; set; }

        public MatchWinLoss BettypesWinLossTotal { get; set; }
    }
}