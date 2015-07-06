// -----------------------------------------------------------------------
// <copyright file="SummarySportsWinLoss.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.ReportingServices.Business.Model.MatchWinLoss2a
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SummarySportsWinLoss
    {
        public SummarySportsWinLoss()
        {
            SummaryWinLossBySports = new List<SummarySportsWinLoss>();
        }

        public string SportName { get; set; }

        public string WinLossDate { get; set; }

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

        public decimal Margin
        {
            get
            {
                decimal margin = 0;
                if (this.IBCTurnover != 0)
                {
                    return this.LineTotal / this.IBCTurnover;
                }

                return margin;
            }
        }

        public bool IsExcludeVirtualLineTotal { get; set; }

        public List<SummarySportsWinLoss> SummaryWinLossBySports { get; set; }

        public SummarySportsWinLoss GrandTotal { get; set; }

        public void AddToGrandTotal(SummarySportsWinLoss winLoss)
        {
            this.GrandTotal.IBCTurnover += winLoss.IBCTurnover;
            this.GrandTotal.WinLoss += winLoss.WinLoss;
            this.GrandTotal.Position += winLoss.Position;
            this.GrandTotal.Commission += winLoss.Commission;
            this.GrandTotal.VirtualCommission += winLoss.VirtualCommission;
            this.GrandTotal.LineTotal += winLoss.LineTotal;
            this.GrandTotal.VirtualLineTotal += winLoss.VirtualLineTotal;
        }
    }
}