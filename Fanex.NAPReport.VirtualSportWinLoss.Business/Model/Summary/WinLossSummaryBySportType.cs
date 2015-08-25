// -----------------------------------------------------------------------
// <copyright file="WinLossSummaryBySportType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="David.Tran" date="2014.05.13">Created</change>
// </history>

namespace Fanex.NAPReport.VirtualSportWinLoss.Business.Model.Summary
{
    using System;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossSummaryBySportType
    {
        public int SportID { get; set; }

        public string SportName { get; set; }

        public int BettypeID { get; set; }

        public string BettypeName { get; set; }

        public int BetCount { get; set; }

        public double AverageValuePerBet
        {
            get
            {
                if (this.BetCount == 0)
                {
                    return 0;
                }

                var result = Converter.To<double>(this.TurnOverIBC / this.BetCount);
                return result;
            }
        }

        public double TurnOverIBC { get; set; }

        public double WinLostIBC { get; set; }

        public double Margin
        {
            get
            {
                if (this.TurnOverIBC == 0)
                {
                    return 0;
                }

                var result = Converter.To<double>(this.WinLostIBC / this.TurnOverIBC);
                return result;
            }
        }
    }
}