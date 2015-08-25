// -----------------------------------------------------------------------
// <copyright file="SummaryBettypeAndMatchAndSportWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SummaryBettypeAndMatchAndSportWinLoss
    {
        public SummaryBettypeAndMatchAndSportWinLoss()
        {
            SummaryBettypesWinLoss = new List<SummaryBettypeWinLoss>();
            MatchAndSportWinLoss = new MatchAndSportWinLoss();
        }

        public List<SummaryBettypeWinLoss> SummaryBettypesWinLoss { get; set; }

        public MatchAndSportWinLoss MatchAndSportWinLoss { get; set; }
    }
}
