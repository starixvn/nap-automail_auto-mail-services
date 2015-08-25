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
    public class SummaryBettypesWinLoss
    {
        public SummaryBettypesWinLoss()
        {
            ReportRows = new List<SummaryBettypesWinLoss>();
        }

        public int BettypeId { get; set; }

        public string BettypeName { get; set; }

        public string Name_Suffix { get; set; }

        public decimal LineTotal_Soccer_Live { get; set; }

        public decimal VirtualLineTotal_Soccer_Live { get; set; }

        public decimal LineTotal_Soccer_Non_Live { get; set; }

        public decimal VirtualLineTotal_Soccer_Non_Live { get; set; }

        public decimal LineTotal_Others_Live { get; set; }

        public decimal VirtualLineTotal_Others_Live { get; set; }

        public decimal LineTotal_Others_Non_Live { get; set; }

        public decimal VirtualLineTotal_Others_Non_Live { get; set; }

        public decimal LineTotalReal
        {
            get
            {
                var lineTotalReal = this.LineTotal_Soccer_Live +
                    this.LineTotal_Soccer_Non_Live +
                    this.LineTotal_Others_Live +
                    this.LineTotal_Others_Non_Live;
                return lineTotalReal;
            }
        }

        public decimal LineTotalVirtual
        {
            get
            {
                var virtualLineTotalReal = this.VirtualLineTotal_Soccer_Live +
                    this.VirtualLineTotal_Soccer_Non_Live +
                    this.VirtualLineTotal_Others_Live +
                    this.VirtualLineTotal_Others_Non_Live;
                return virtualLineTotalReal;
            }
        }

        public decimal Variance
        {
            get
            {
                decimal result = 0;
                if (IsExcludeVirtualLineTotal)
                {
                    result = 0;
                }
                else
                {
                    result = this.LineTotalReal - this.LineTotalVirtual;
                }

                return result;
            }
        }

        public string Hedge { get; set; }

        public decimal Turnover { get; set; }

        public decimal Margin
        {
            get
            {
                decimal margin = 0;
                if (this.Turnover != 0)
                {
                    return this.LineTotalReal / this.Turnover * 100;
                }

                return margin;
            }
        }

        public bool IsExcludeVirtualLineTotal { get; set; }

        public List<SummaryBettypesWinLoss> ReportRows { get; set; }

        public SummaryBettypesWinLoss GrandTotal { get; set; }

        public void AddToGrandTotal(SummaryBettypesWinLoss winLoss)
        {
            bool isExcludeVirtualLineTotal = winLoss.IsExcludeVirtualLineTotal;
            this.GrandTotal.LineTotal_Soccer_Live += winLoss.LineTotal_Soccer_Live;
            this.GrandTotal.VirtualLineTotal_Soccer_Live += isExcludeVirtualLineTotal ? 0 : winLoss.VirtualLineTotal_Soccer_Live;
            this.GrandTotal.LineTotal_Soccer_Non_Live += winLoss.LineTotal_Soccer_Non_Live;
            this.GrandTotal.VirtualLineTotal_Soccer_Non_Live += isExcludeVirtualLineTotal ? 0 : winLoss.VirtualLineTotal_Soccer_Non_Live;
            this.GrandTotal.LineTotal_Others_Live += winLoss.LineTotal_Others_Live;
            this.GrandTotal.VirtualLineTotal_Others_Live += isExcludeVirtualLineTotal ? 0 : winLoss.VirtualLineTotal_Others_Live;
            this.GrandTotal.LineTotal_Others_Non_Live += winLoss.LineTotal_Others_Non_Live;
            this.GrandTotal.VirtualLineTotal_Others_Non_Live += isExcludeVirtualLineTotal ? 0 : winLoss.VirtualLineTotal_Others_Non_Live;
            this.GrandTotal.Turnover += winLoss.Turnover;
        }
    }
}