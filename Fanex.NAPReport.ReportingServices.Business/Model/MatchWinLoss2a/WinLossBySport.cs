// -----------------------------------------------------------------------
// <copyright file="WinLossBySport.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossBySport
    {
        public WinLossBySport()
        {
            BettypesWinLossBySport = new List<BettypeWinLoss>();
            SportWinLossTotal = new MatchWinLoss();
        }

        public string SportName { get; set; }

        public List<BettypeWinLoss> BettypesWinLossBySport { get; set; }

        public MatchWinLoss SportWinLossTotal { get; set; }
    }
}