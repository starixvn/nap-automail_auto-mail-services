// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss
    {
        public string MatchCode { get; set; }

        public string KickOffTime { get; set; }

        public bool LiveIndicator { get; set; }

        public int BettypeId { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Position { get; set; }

        public decimal Commission { get; set; }

        public decimal VirtualCommission { get; set; }

        public decimal LineTotal { get; set; }

        public decimal VirtualLineTotal { get; set; }

        public decimal IBCTurnover { get; set; }

        public string LeagueName { get; set; }

        public bool IsNeutral { get; set; }
    }
}