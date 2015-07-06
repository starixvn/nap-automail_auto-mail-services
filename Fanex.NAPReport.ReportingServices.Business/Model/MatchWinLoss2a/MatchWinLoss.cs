// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLoss
    {
        public string MatchCode { get; set; }

        public string EventDate { get; set; }

        public string KickOffTime { get; set; }

        public bool LiveIndicator { get; set; }

        public int BettypeId { get; set; }

        public string BettypeName { get; set; }

        public int HomeTeamId { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeamId { get; set; }

        public string AwayTeamName { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Position { get; set; }

        public decimal Commission { get; set; }

        public decimal VirtualCommission { get; set; }

        public decimal LineTotal { get; set; }

        public decimal VirtualLineTotal { get; set; }

        public decimal IBCTurnover { get; set; }

        public decimal Variance
        {
            get
            {
                return this.LineTotal - this.VirtualLineTotal;
            }
        }

        public decimal Turnover { get; set; }

        public decimal Margin
        {
            get
            {
                decimal margin = 0;

                if (this.Turnover != 0)
                {
                    return this.LineTotal / this.Turnover;
                }

                return margin;
            }
        }

        public string LeagueName { get; set; }

        public bool IsNeutral { get; set; }

        /// <summary>
        /// Adds the specified win loss.
        /// </summary>
        /// <param name="winLoss">The win loss.</param>
        public void Add(MatchWinLoss winLoss)
        {
            this.Turnover += winLoss.Turnover;
            this.WinLoss += winLoss.WinLoss;
            this.Position += winLoss.Position;
            this.Commission += winLoss.Commission;
            this.VirtualCommission += winLoss.VirtualCommission;
            this.LineTotal += winLoss.LineTotal;
            this.VirtualLineTotal += winLoss.VirtualLineTotal;
        }
    }
}