// -----------------------------------------------------------------------
// <copyright file="SMAWinLossDetail.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.SMAReport.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAWinLossDetail
    {
        public int Month { get; set; }

        public decimal TurnOver { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Margin
        {
            get
            {
                decimal margin = this.TurnOver == 0 ? 0 :
                                this.WinLoss / this.TurnOver;

                return margin;
            }
        }

        public void CalculateTotal(SMAWinLossDetail smaReportDetail)
        {
            this.TurnOver += smaReportDetail.TurnOver;
            this.WinLoss += smaReportDetail.WinLoss;
        }
    }
}
