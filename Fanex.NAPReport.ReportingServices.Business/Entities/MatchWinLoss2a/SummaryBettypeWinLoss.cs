// -----------------------------------------------------------------------
// <copyright file="SummaryBettypeWinLoss.cs" company="Nexcel Solutions Vietnam">
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
    public class SummaryBettypeWinLoss
    {
        public int BettypeId { get; set; }

        public string BettypeName { get; set; }

        public decimal LineTotal_Soccer_Live { get; set; }

        public decimal VirtualLineTotal_Soccer_Live { get; set; }

        public decimal LineTotal_Soccer_Non_Live { get; set; }

        public decimal VirtualLineTotal_Soccer_Non_Live { get; set; }

        public decimal LineTotal_Others_Live { get; set; }

        public decimal VirtualLineTotal_Others_Live { get; set; }

        public decimal LineTotal_Others_Non_Live { get; set; }

        public decimal VirtualLineTotal_Others_Non_Live { get; set; }

        public string Hedge { get; set; }

        public decimal Turnover { get; set; }
    }
}
