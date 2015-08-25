// -----------------------------------------------------------------------
// <copyright file="SportWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Entities.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SportWinLoss
    {
        public string SportName { get; set; }

        public string WinLossDate { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Position { get; set; }

        public decimal Commission { get; set; }

        public decimal VirtualCommission { get; set; }

        public decimal LineTotal { get; set; }

        public decimal VirtualLineTotal { get; set; }

        public decimal IBCTurnover { get; set; }
    }
}