// -----------------------------------------------------------------------
// <copyright file="BettypeWinLossInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BettypeWinLoss
    {
        public string Bettype { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Turnover { get; set; }

        public int BetCount { get; set; }

        public int MatchCount { get; set; }

        public decimal Margin
        {
            get
            {
                return this.Turnover == 0 ? 0 : this.WinLoss / this.Turnover;
            }
        }

        public void CalculateTotal(BettypeWinLoss bettypeWinLoss)
        {
            this.WinLoss += bettypeWinLoss.WinLoss;
            this.Turnover += bettypeWinLoss.Turnover;
            this.BetCount += bettypeWinLoss.BetCount;
        }
    }
}
