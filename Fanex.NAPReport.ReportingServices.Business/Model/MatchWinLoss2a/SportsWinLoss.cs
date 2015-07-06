// -----------------------------------------------------------------------
// <copyright file="SportsWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SportsWinLoss
    {
        public SportsWinLoss()
        {
            WinLossBySports = new List<WinLossBySport>();
            GrandTotal = new MatchWinLoss();
            EightRMTotal = new MatchWinLoss();
            BARMTotal = new MatchWinLoss();
        }

        public List<WinLossBySport> WinLossBySports { get; set; }

        public MatchWinLoss GrandTotal { get; set; }

        public MatchWinLoss EightRMTotal { get; set; }

        public MatchWinLoss BARMTotal { get; set; }
    }
}